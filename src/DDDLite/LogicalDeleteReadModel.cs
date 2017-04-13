namespace DDDLite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class LogicalDeleteReadModel : AggregateReadModel, ILogicalDelete
    {
        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool Deleted { get; set; }
    }
}
