using DDDLite.Querying;
using System.Collections.Generic;

namespace DDDLite.Actors
{
    public class PagedInputForm : FindAllInputForm
    {
        public PagedInputForm()
        {
        }

        public PagedInputForm(int page, int limit, List<Filter> filters, List<Sorter> sorters)
        {
            this.PageIndex = page;
            this.PageSize = limit;
            this.Filters = filters;
            this.Sorters = sorters;
        }

        public PagedInputForm(int page, int limit, List<Filter> filters, List<Sorter> sorters, List<string> eagerLoadings)
        {
            this.PageIndex = page;
            this.PageSize = limit;
            this.Filters = filters;
            this.Sorters = sorters;
            this.EagerLoadings = eagerLoadings;
        }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
