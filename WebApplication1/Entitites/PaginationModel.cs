namespace WebApplication1.Entitites
{
    public class PaginationModel
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string? SortBy { get; set; }
        public string SortOrder { get; set; } = "asc";
    }
}
