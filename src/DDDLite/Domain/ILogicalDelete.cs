namespace DDDLite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ILogicalDelete
    {
        bool Deleted { get; set; }
    }
}
