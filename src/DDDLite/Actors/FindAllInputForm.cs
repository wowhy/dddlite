namespace DDDLite.Actors
{
    using System.Collections.Generic;

    using Querying;

    public class FindAllInputForm
    {
        public List<Sorter> Sorters { get; set; }

        public List<Filter> Filters { get; set; }

        public List<string> EagerLoadings { get; set; }

        public FindAllInputForm()
        {
        }

        public FindAllInputForm(List<Filter> filters, List<Sorter> sorters)
        {
            this.Filters = filters;
            this.Sorters = sorters;
        }

        public FindAllInputForm(List<Filter> filters, List<Sorter> sorters, List<string> eagerLoadings)
        {
            this.Filters = filters;
            this.Sorters = sorters;
            this.EagerLoadings = eagerLoadings;
        }
    }
}
