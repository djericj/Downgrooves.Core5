﻿using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Extensions
{
    public static class ITunesLoaderExtensions
    {
        public static Release ToRelease(this ITunesCollection item)
        {
            return new Release()
            {
                ArtistName = item.ArtistName,
                ArtistViewUrl = item.ArtistViewUrl,
                Copyright = item.Copyright,
                Country = item.Country,
                ReleaseDate = item.ReleaseDate,
                SourceSystemId = item.CollectionId,
                BuyUrl = item.CollectionViewUrl,
                Genre = item.PrimaryGenreName,
                Price = item.CollectionPrice,
                Title = item.CollectionCensoredName
            };
        }

        public static Release ToRelease(this ITunesLookupResultItem item)
        {
            return new Release()
            {
                ArtistName = item.ArtistName,
                ArtistViewUrl = item.ArtistViewUrl,
                Copyright = item.Copyright,
                Country = item.Country,
                DiscCount = item.DiscCount,
                DiscNumber = item.DiscNumber,
                PreviewUrl = item.PreviewUrl,
                ReleaseDate = item.ReleaseDate,
                SourceSystemId = item.CollectionId,
                BuyUrl = item.CollectionViewUrl,
                Genre = item.PrimaryGenreName,
                Price = item.CollectionPrice,
                Title = item.CollectionCensoredName,
                VendorId = 1
            };
        }

        public static IList<Release> ToReleases(this IEnumerable<ITunesLookupResultItem> items)
        {
            var releases = new List<Release>();
            foreach (var item in items)
                releases.Add(item.ToRelease());
            return releases;
        }

        public static ReleaseTrack ToReleaseTrack(this ITunesTrack item, int releaseId)
        {
            return new ReleaseTrack()
            {
                ArtistName = item.ArtistName,
                Price = item.TrackPrice,
                Title = item.TrackCensoredName,
                PreviewUrl = item.PreviewUrl,
                TrackNumber = item.TrackNumber,
                TrackTimeInMilliseconds = item.TrackTimeMillis,
                SourceSystemId = item.TrackId,
                ReleaseId = releaseId
            };
        }

        public static IEnumerable<ReleaseTrack> ToReleaseTracks(this IEnumerable<ITunesTrack> items, int releaseId)
        {
            var tracks = new List<ReleaseTrack>();
            foreach (var item in items)
                tracks.Add(item.ToReleaseTrack(releaseId));
            return tracks;
        }

        public static ReleaseTrack ToReleaseTrack(this ITunesLookupResultItem item, int releaseId)
        {
            return new ReleaseTrack()
            {
                ArtistName = item.ArtistName,
                Price = item.TrackPrice,
                Title = item.TrackCensoredName,
                PreviewUrl = item.PreviewUrl,
                TrackNumber = item.TrackNumber,
                TrackTimeInMilliseconds = item.TrackTimeMillis,
                SourceSystemId = item.TrackId,
                ReleaseId = releaseId
            };
        }

        public static IList<ReleaseTrack> ToReleaseTracks(this IEnumerable<ITunesLookupResultItem> items, int releaseId)
        {
            var tracks = new List<ReleaseTrack>();
            foreach (var item in items)
                tracks.Add(item.ToReleaseTrack(releaseId));
            return tracks;
        }

        public static IList<ITunesCollection> ToITunesCollections(this IEnumerable<ITunesLookupResultItem> items)
        {
            var collections = new List<ITunesCollection>();
            foreach (var item in items)
            {
                collections.Add(new ITunesCollection()
                {
                    ArtistId = item.ArtistId,
                    ArtistName = item.ArtistName,
                    ArtistViewUrl = item.ArtistViewUrl,
                    ArtworkUrl100 = item.ArtworkUrl100,
                    ArtworkUrl60 = item.ArtworkUrl60,
                    CollectionCensoredName = item.CollectionCensoredName,
                    CollectionExplicitness = item.CollectionExplicitness,
                    CollectionId = item.CollectionId,
                    CollectionName = item.CollectionName,
                    CollectionPrice = item.CollectionPrice,
                    CollectionType = item.CollectionType,
                    CollectionViewUrl = item.CollectionViewUrl,
                    Copyright = item.Copyright,
                    Country = item.Country,
                    Currency = item.Currency,
                    TrackCount = item.TrackCount,
                    WrapperType = item.WrapperType,
                    PrimaryGenreName = item.PrimaryGenreName,
                    ReleaseDate = item.ReleaseDate
                });
            }
            return collections;
        }

        public static IList<ITunesTrack> ToITunesTracks(this IEnumerable<ITunesLookupResultItem> items)
        {
            var tracks = new List<ITunesTrack>();
            foreach (var item in items)
            {
                tracks.Add(new ITunesTrack()
                {
                    ArtistId = item.ArtistId,
                    ArtistName = item.ArtistName,
                    ArtistViewUrl = item.ArtistViewUrl,
                    ArtworkUrl100 = item.ArtworkUrl100,
                    ArtworkUrl30 = item.ArtworkUrl30,
                    ArtworkUrl60 = item.ArtworkUrl60,
                    CollectionCensoredName = item.CollectionCensoredName,
                    CollectionExplicitness = item.CollectionExplicitness,
                    CollectionId = item.CollectionId,
                    CollectionName = item.CollectionName,
                    CollectionPrice = item.CollectionPrice,
                    CollectionViewUrl = item.CollectionViewUrl,
                    Country = item.Country,
                    Currency = item.Currency,
                    DiscCount = item.DiscCount,
                    TrackCensoredName = item.TrackCensoredName,
                    DiscNumber = item.DiscNumber,
                    IsStreamable = item.IsStreamable,
                    Kind = item.Kind,
                    PreviewUrl = item.PreviewUrl,
                    PrimaryGenreName = item.PrimaryGenreName,
                    ReleaseDate = item.ReleaseDate,
                    TrackCount = item.TrackCount,
                    TrackExplicitness = item.TrackExplicitness,
                    TrackId = item.TrackId,
                    TrackName = item.TrackName,
                    TrackNumber = item.TrackNumber,
                    TrackPrice = item.TrackPrice,
                    TrackTimeMillis = item.TrackTimeMillis,
                    TrackViewUrl = item.TrackViewUrl,
                    WrapperType = item.WrapperType,
                });
            }
            return tracks;
        }
    }
}