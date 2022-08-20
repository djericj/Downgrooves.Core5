using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Downgrooves.Domain.ITunes;
using Downgrooves.Service.Base;
using Microsoft.Extensions.Configuration;

namespace Downgrooves.Service
{
    public class ReleaseService : ServiceBase, IReleaseService
    {
        public ReleaseService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public Release Add(Release release)
        {
            _unitOfWork.Releases.AddRelease(release);
            _unitOfWork.Complete();
            return release;
        }

        public ReleaseTrack AddTrack(ReleaseTrack releaseTrack)
        {
            _unitOfWork.ReleaseTracks.Add(releaseTrack);
            _unitOfWork.Complete();
            return releaseTrack;
        }

        public IEnumerable<ReleaseTrack> AddTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            _unitOfWork.ReleaseTracks.AddRange(releaseTracks);
            _unitOfWork.Complete();
            return releaseTracks;
        }

        public List<ITunesExclusion> GetExclusions() => _unitOfWork.Releases.Exclusions;

        public IEnumerable<Release> GetReleases(Expression<Func<Release, bool>> predicate)
        {
            return _unitOfWork.Releases.Find(predicate);
        }

        public IEnumerable<Release> GetReleases(string artistName = null)
        {
            return _unitOfWork.Releases.GetReleases(artistName);
        }

        public IEnumerable<Release> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false)
        {
            return _unitOfWork.Releases.GetReleases(parameters, artistName, artistId, isOriginal, isRemix);
        }

        public ReleaseTrack GetReleaseTrack(int id)
        {
            var releaseTrack = _unitOfWork.ReleaseTracks.Find(x => x.Id == id);
            return releaseTrack?.FirstOrDefault();
        }

        public void Remove(int id)
        {
            var release = GetReleases(x => x.Id == id);
            _unitOfWork.Releases.Remove(release.FirstOrDefault());
            _unitOfWork.Complete();
        }

        public void RemoveTrack(int id)
        {
            var track = GetReleaseTrack(id);
            _unitOfWork.ReleaseTracks.Remove(track);
            _unitOfWork.Complete();
        }

        public void RemoveTracks(IEnumerable<int> ids)
        {
            foreach (var id in ids)
                RemoveTrack(id);
        }

        public Release Update(Release release)
        {
            _unitOfWork.Releases.UpdateState(release);
            _unitOfWork.Complete();
            return release;
        }

        public ReleaseTrack UpdateTrack(ReleaseTrack releaseTrack)
        {
            _unitOfWork.ReleaseTracks.UpdateState(releaseTrack);
            _unitOfWork.Complete();
            return releaseTrack;
        }

        public IEnumerable<ReleaseTrack> UpdateTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            foreach (var item in releaseTracks)
                UpdateTrack(item);
            return releaseTracks;
        }
    }
}