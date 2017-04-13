namespace DDDLite
{
    public abstract class LogicalDeleteAggregateRoot : AggregateRoot, ILogicalDelete
    {
        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool Deleted { get; set; }
    }
}
