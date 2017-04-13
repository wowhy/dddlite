namespace DDDLite.Actors
{
    using Querying;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FindSingleInputForm
    {
        public List<Sorter> Sorters { get; set; }

        public List<Filter> Filters { get; set; }

        public List<string> EagerLoadings { get; set; }

        public FindSingleInputForm()
        {
        }

        public FindSingleInputForm(List<Filter> filters, List<Sorter> sorters)
        {
            this.Filters = filters;
            this.Sorters = sorters;
        }

        public FindSingleInputForm(List<Filter> filters, List<Sorter> sorters, List<string> eagerLoadings)
        {
            this.Filters = filters;
            this.Sorters = sorters;
            this.EagerLoadings = eagerLoadings;
        }
    }
}
