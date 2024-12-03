namespace Doantour.Helpers.Page
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage
        {
            get
            {
                if (TotalCount == 0) return 0;
                var total = TotalCount / PageSize;
                if (TotalCount % PageSize != 0)

                    total++;
                return total;
            }
        }
        public Pagination()
        {
            //PageSize = -1;            
            PageSize = int.MaxValue;
            PageNumber = 1;
        }
    }
}
