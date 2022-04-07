﻿using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IYouTubeService
    {
        Task AddNewVideos();

        Task<IEnumerable<Video>> GetYouTubeVideosJson();
    }
}