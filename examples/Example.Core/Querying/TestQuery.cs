namespace Example.Core.Querying
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite;

    public class Test : IConcurrencyVersion
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public int TestNum { get; set; }

        public long RowVersion
        {
            get; set;
        }
    }
}