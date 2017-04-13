using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDLite.Auth
{
    public class RBACUser : Operator
    {
        public Guid Id { get; set; }
        public ICollection<RBACRole> Roles { get; set; } = new List<RBACRole>();
    }
}
