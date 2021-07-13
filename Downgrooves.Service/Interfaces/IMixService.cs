﻿using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IMixService
    {
        IEnumerable<Mix> GetMixes();

        Task<IEnumerable<Mix>> GetMixesAsync();

        IEnumerable<Mix> GetMixesByCategory(string category);

        IEnumerable<Mix> GetMixesByGenre(string genre);
    }
}