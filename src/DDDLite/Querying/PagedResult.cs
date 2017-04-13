namespace DDDLite.Querying
{
    using System.Collections.Generic;

    public class PagedResult<TData>
    {
        public int Total { get; set; }

        public List<TData> Data { get; set; }
    }
}
