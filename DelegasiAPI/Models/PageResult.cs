namespace DelegasiAPI.Models
{
    public class PageResult<T>
    {
        public PageResult()
        {

        }

        public PageResult(IEnumerable<T> data, int pageIndex, int pageSize, int totalCount)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public PageResult(int totalCount, IEnumerable<T> data, PageFilter filter)
        {
            Data = data;
            PageIndex = filter.PageIndex;
            PageSize = filter.PageSize;
            TotalCount = totalCount;
        }
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
