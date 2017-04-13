using DDDLite.Commands;
using Example.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Core.Commands
{
    // CreateCommand<User>
    // UpdateCommand<User>
    // DeleteCommand<User>

    public class ChangePasswordCommand : AggregateRootCommand<User>
    {
        public string NewPassword { get; set; }

        public string OldPassword { get; set; }
    }
}
