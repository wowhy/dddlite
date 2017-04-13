using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDLite.Auth
{
    public class RBACRole
    {
        public string Application { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<RBACPermission> Permissions { get; set; } = new List<RBACPermission>();
    }
}
