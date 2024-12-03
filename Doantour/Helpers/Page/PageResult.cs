namespace Doantour.Helpers.Page
{
    public class PageResult<T>
    {
        public Pagination Pagination { get; set; }
        public IEnumerable<T> Data { get; set; }
        public PageResult(Pagination pagination, IEnumerable<T> data)
        {
            Pagination = pagination;
            Data = data;
        }
        public static IEnumerable<T> ToPageResult(Pagination pagination, IEnumerable<T> data)
        {
            pagination.PageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
            data = data.Skip(pagination.PageSize * (pagination.PageNumber - 1)).Take(pagination.PageSize).AsEnumerable();
            return data;
        }
    }
}
