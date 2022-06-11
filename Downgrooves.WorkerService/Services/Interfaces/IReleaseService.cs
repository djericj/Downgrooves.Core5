﻿using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IReleaseService
    {
        Task<Release> AddNewRelease(Release release);

        Task<ReleaseTrack> AddNewReleaseTrack(ReleaseTrack releaseTrack);

        Task<int> AddNewReleases(IEnumerable<Release> releases);

        Task<int> AddNewReleaseTracks(IEnumerable<ReleaseTrack> releaseTracks);

        Task<IEnumerable<Release>> GetReleases(Artist artist = null);
    }
}