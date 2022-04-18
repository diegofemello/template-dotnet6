namespace Infrastructure.Helpers
{
    public class PageParams
    {
        private const int MaxPageSize = 200;
        public int PageNumber { get; set; } = 1;
        private int pageSize;
        public int PageSize
        {
            get { return pageSize <= 0 ? MaxPageSize : pageSize;  }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string Term { get; set; } = string.Empty;
    }
}
