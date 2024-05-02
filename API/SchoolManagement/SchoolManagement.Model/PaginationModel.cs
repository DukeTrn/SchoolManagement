namespace SchoolManagement.Model
{
    public sealed class PaginationModel<T>
    {
        public int? TotalCount { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public int? TotalPageCount
        {
            get
            {
                if (!(TotalCount >= 0) || !(PageSize >= 1))
                {
                    return null;
                }
                var totalPageCount = 1.0 * TotalCount / PageSize;
                return (int)Math.Ceiling(totalPageCount.Value);
            }
        }

        public IEnumerable<T> DataList { get; set; } = null!;
    }
}