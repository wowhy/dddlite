using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDLite;
using System.ComponentModel.DataAnnotations.Schema;
using DDDLite.Querying;

namespace Example.Core.Querying
{
    public class User : IConcurrencyVersion
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Guid? EnabledById { get; set; }

        [EagerLoading]
        public User EnabledBy { get; set; }

        public long RowVersion
        {
            get; set;
        }
    }
}
