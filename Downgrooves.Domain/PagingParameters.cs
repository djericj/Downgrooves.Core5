namespace Downgrooves.Domain
{
    /// <summary>
    /// Represents a collection of parameters to page data results.
    /// </summary>
    public class PagingParameters
    {
        private const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}