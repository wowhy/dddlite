using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDLite.Auth
{
    public class RBACPermission
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<RBACRole> Roles { get; set; } = new List<RBACRole>();
    }
}
