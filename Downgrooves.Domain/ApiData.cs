using System;

namespace Downgrooves.Domain
{
    /// <summary>
    /// Raw API data
    /// </summary>
    public partial class ApiData
    {
        public ApiDataTypes ApiDataType { get; set; }
        public string Artist { get; set; }
        public int CollectionId { get; set; }
        public string Data { get; set; }
        public int Id { get; set; }
        public bool IsChanged { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime LastChecked { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Total { get; set; }
        public string Url { get; set; }
    }
}