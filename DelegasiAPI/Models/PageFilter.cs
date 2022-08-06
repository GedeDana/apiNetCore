namespace DelegasiAPI.Models
{
    public class PageFilter
    {
        public PageFilter()
        {

        }

        public PageFilter(string orderBy)
        {
        }

        public const int DefaultPageSize = 25;

        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 0;

        internal int SkipSize
        {
            get { return PageIndex * PageSize; }
        }
    }
}