using System;
using System.ComponentModel.DataAnnotations;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public abstract class ITunesViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = null)]
        public string WrapperType { get; set; }

        [Required(ErrorMessage = null)]
        public int ArtistId { get; set; }

        [Required(ErrorMessage = null)]
        public string ArtistName { get; set; }

        [Required(ErrorMessage = null)]
        public int CollectionId { get; set; }

        [Required(ErrorMessage = null)]
        public string CollectionName { get; set; }

        [Required(ErrorMessage = null)]
        public string CollectionCensoredName { get; set; }

        [Required(ErrorMessage = null)]
        public string ArtistViewUrl { get; set; }

        [Required(ErrorMessage = null)]
        public string CollectionViewUrl { get; set; }

        [Required(ErrorMessage = null)]
        public string ArtworkUrl60 { get; set; }

        [Required(ErrorMessage = null)]
        public string ArtworkUrl100 { get; set; }

        [Required(ErrorMessage = null)]
        public double CollectionPrice { get; set; }

        [Required(ErrorMessage = null)]
        public string CollectionExplicitness { get; set; }

        [Required(ErrorMessage = null)]
        public int TrackCount { get; set; }

        [Required(ErrorMessage = null)]
        public string Country { get; set; }

        [Required(ErrorMessage = null)]
        public string Currency { get; set; }

        [Required(ErrorMessage = null)]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = null)]
        public string PrimaryGenreName { get; set; }

        [Required(ErrorMessage = null)]
        public int SourceArtistId { get; set; }
    }
}