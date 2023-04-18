using System;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IITunesService
    {
        void CheckFolders();
        void GetData();
        void GetDataFromITunesApi();
        void GetArtwork();
        DateTime GetLastCheckedFile();
        void WriteLastCheckedFile();
    }
}