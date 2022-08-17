using System;

namespace Downgrooves.Domain
{
    /// <summary>
    /// Raw API data
    /// </summary>
    public partial class ApiData
    {
        public string Artist { get; set; }
        public string Data { get; set; }
        public ApiDataTypes ApiDataType { get; set; }
        public int Id { get; set; }
        public bool IsChanged { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Total { get; set; }
        public string Url { get; set; }
    }
}