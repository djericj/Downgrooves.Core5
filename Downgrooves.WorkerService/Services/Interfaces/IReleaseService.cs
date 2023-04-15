using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IReleaseService
    {
        void ProcessReleases();

        Release Add(Release release);

        IEnumerable<Release> Add(IEnumerable<Release> releases);

        ReleaseTrack AddTrack(ReleaseTrack releaseTrack);

        IEnumerable<ReleaseTrack> AddTracks(IEnumerable<ReleaseTrack> releaseTracks);

        IEnumerable<Release> GetReleases(string artistName = null);

        ReleaseTrack GetReleaseTrack(int id);

        Release Update(Release release);

        ReleaseTrack UpdateTrack(ReleaseTrack releaseTrack);

        IEnumerable<ReleaseTrack> UpdateTracks(IEnumerable<ReleaseTrack> releaseTracks);
    }
}