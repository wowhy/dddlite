using System;
using DDDLite;

namespace TestCommand.Aggregates
{
    public class User : AggregateRoot
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
