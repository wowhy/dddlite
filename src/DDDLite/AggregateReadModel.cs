using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDLite
{
    public abstract class AggregateReadModel : IConcurrencyVersion
    {
        public Guid Id { get; set; }

        public Guid? CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long RowVersion { get; set; }
    }
}
