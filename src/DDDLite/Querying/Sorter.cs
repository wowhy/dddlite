namespace DDDLite.Querying
{
    using Domain;

    public sealed class Sorter
    {
        public string Property { get; set; }

        public SortDirection Direction { get; set; }

        public Sorter() { }

        public Sorter(string property) : this(property, SortDirection.Asc) { }

        public Sorter(string property, SortDirection direction)
        {
            this.Property = property;
            this.Direction = direction;
        }
    }
}