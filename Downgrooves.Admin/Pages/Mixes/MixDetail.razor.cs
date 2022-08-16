using Downgrooves.Admin.Shared;
using Downgrooves.Domain;
using Downgrooves.Admin.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Pages.Mixes
{
    public class MixDetailModel : BaseComponent
    {
        protected Stream _fileStream = null;
        protected string _selectedFileName = null;

        protected bool _uploading;
        protected long _uploadedBytes = 0;

        protected long _percentage = 0;
        protected string echo;

        [Inject]
        public MixViewModel MixViewModel { get; set; }

        [Inject]
        public MixTrackViewModel MixTrackViewModel { get; set; }

        public IBrowserFile ArtworkFile { get; set; }

        public IBrowserFile AudioFile { get; set; }

        protected override async Task OnInitializedAsync()
        {
            RedirectUrl = "/mixes";
            ViewModel = MixViewModel;
            BreadcrumbItems.Clear();
            BreadcrumbItems.Add(new Breadcrumb($"/mixes", "Mixes"));
            if (Id.HasValue)
            {
                await MixViewModel.GetMix(Id.Value);
                BreadcrumbItems.Add(new Breadcrumb($"/mix/{Id}", MixViewModel.Title));
            }
            await base.OnInitializedAsync();
        }

        protected async Task AddTrack()
        {
            try
            {
                MixTrackViewModel.MixId = Id.Value;
                await MixTrackViewModel.Add();
                await MixViewModel.GetMix(Id.Value);
                StateHasChanged();
                ToastService.ShowSuccess("Track added successfully!");
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        protected async Task DeleteTrack(int id)
        {
            try
            {
                await MixTrackViewModel.Remove(id);
                await MixViewModel.GetMix(Id.Value);
                StateHasChanged();
                ToastService.ShowSuccess("Track deleted successfully!");
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        protected async void DeleteImage()
        {
            if (!await JsRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the image?"))
                return;

            await MixViewModel.RemoveArtwork();
            this.StateHasChanged();
        }

        protected async void UploadImage()
        {
            var data = await Read(ArtworkFile.OpenReadStream());
            var mediaFile = new MediaFile()
            {
                FileName = ArtworkFile.Name,
                Data = data
            };
            await MixViewModel.AddArtwork(mediaFile);
            MixViewModel.ArtworkUrl = ArtworkFile.Name;
            await MixViewModel.Update();
            await MixViewModel.GetMix(Id.Value);
            StateHasChanged();
        }

        protected async Task<byte[]> Read(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            stream.Close();

            var data = ms.ToArray();
            ms.Close();
            return data;
        }

        protected void OnImageInputFileChange(InputFileChangeEventArgs e)
        {
            ArtworkFile = e.File;
            this.StateHasChanged();
        }

        protected async void DeleteAudio()
        {
            if (!await JsRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to delete the audio?"))
                return;

            await MixViewModel.RemoveAudio();
            this.StateHasChanged();
        }

        protected async void UploadAudio()
        {
            MixViewModel.AudioUrl = AudioFile.Name;
            await MixViewModel.Update();
            await MixViewModel.GetMix(Id.Value);
            StateHasChanged();
        }

        protected async Task OnAudioInputFileChange(InputFileChangeEventArgs e)
        {
            const long CHUNKSIZE = 1024 * 400; // subjective

            var file = e.File;
            long totalBytes = file.Size;
            int fragment = 0;
            long chunkSize;

            using (var inStream = file.OpenReadStream(long.MaxValue))
            {
                _uploading = true;
                while (_uploading)
                {
                    chunkSize = CHUNKSIZE;
                    if (_uploadedBytes + CHUNKSIZE > totalBytes)
                    {// remainder
                        chunkSize = totalBytes - _uploadedBytes;
                    }
                    var chunk = new byte[chunkSize];
                    await inStream.ReadAsync(chunk, 0, chunk.Length);
                    // upload this fragment
                    using var formFile = new MultipartFormDataContent();
                    var fileContent = new StreamContent(new MemoryStream(chunk));
                    formFile.Add(fileContent, "file", file.Name);
                    // post
                    await MixViewModel.AddAudioChunk(fragment, formFile);
                    // Update our progress data and UI
                    _uploadedBytes += chunkSize;
                    _percentage = _uploadedBytes * 100 / totalBytes;
                    echo = $"Uploaded {_percentage}%  {_uploadedBytes} of {totalBytes} | Fragment: {fragment}";
                    if (_percentage >= 100)
                    {// upload complete
                        _uploading = false;
                    }
                    await InvokeAsync(StateHasChanged);
                }
            }
            AudioFile = file;
            this.StateHasChanged();
        }

        public class ProgressiveStreamContent : StreamContent
        {
            private readonly Stream _fileStream;

            // Maximum amount of bytes to send per packet
            private readonly int _maxBuffer = 1024 * 4;

            public ProgressiveStreamContent(Stream stream, int maxBuffer, Action<long, double> onProgress) : base(stream)
            {
                _fileStream = stream;
                _maxBuffer = maxBuffer;
                OnProgress += onProgress;
            }

            /// <summary>
            /// Event that we can subscribe to which will be triggered everytime a part of the file is uploaded.
            /// It passes the total amount of uploaded bytes and the percentage.
            /// </summary>
            public event Action<long, double> OnProgress;

            // Override the SerialzeToStreamAsync method, which provides us with the stream that we can write our chunks into.
            protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                // Define an array of bytes with the the length of the maximum amount of bytes to be pushed.
                var buffer = new byte[_maxBuffer];
                var totalLength = _fileStream.Length;
                // Variable that holds the amount of uploaded bytes
                long uploaded = 0;

                // Create a while loop that we will break internally when all bytes uploaded to the server.
                while (true)
                {
                    using (_fileStream)
                    {
                        // In this part of code here we read a chunk of bytes in every loop and write them to the stream of HttpContent.
                        var length = await _fileStream.ReadAsync(buffer, 0, _maxBuffer);
                        // Check if the amount of bytes read recently, if there are no bytes read, break the loop.
                        if (length <= 0)
                            break;

                        // Add the amount of read bytes to uploaded variable.
                        uploaded += length;
                        // Calculate the percentage of the uploaded bytes out of the total remaining.
                        var percentage = Convert.ToDouble(uploaded * 100 / _fileStream.Length);

                        // Write the bytes to the HttpContent stream.
                        await stream.WriteAsync(buffer);

                        // Fire the event of OnProgress to notify the client about progress.
                        OnProgress?.Invoke(uploaded, percentage);

                        // Add this delay over here just to simulate the progress, because locally it's going to be too fast to notice.
                        await Task.Delay(250);
                    }
                }
            }
        }
    }
}