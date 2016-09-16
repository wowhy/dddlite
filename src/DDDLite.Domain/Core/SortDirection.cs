namespace DDDLite.Domain.Core
{
    /// <summary>
    /// Represents the sort order in a sorted query.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Indicates that the sort is unspecified.
        /// </summary>
        Undefined = -1,

        /// <summary>
        /// Indicates an ascending sort.
        /// </summary>
        Asc = 0,

        /// <summary>
        /// Indicates a descending sort.
        /// </summary>
        Desc = 1
    }
}
