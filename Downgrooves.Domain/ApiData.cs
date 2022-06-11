using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    [Table("apiData")]
    public class ApiData
    {
        private bool isChanged;

        public enum ApiDataType
        {
            iTunesCollection = 1,
            iTunesTrack = 2,
            YouTubeVideos = 3
        }

        [Key]
        public int ApiDataId { get; set; }

        [Column("apiDataType")]
        public ApiDataType DataType { get; set; }

        public string Artist { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }
        public int Total { get; set; }
        public bool IsChanged { get => isChanged; set => isChanged = value; }
        public DateTime LastUpdate { get; set; }
    }
}