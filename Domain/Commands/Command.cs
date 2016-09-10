namespace Domain.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;

    public class Command : ICommand
    {
        public Guid Id { get; set; } = SequentialGuid.Create(SequentialGuidType.SequentialAsString);
    }
}
