namespace DDDLite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PagedResult<TData>
    {
        public int Total { get; set; }

        public List<TData> Data { get; set; }
    }
}
