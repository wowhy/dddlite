namespace Example.Core.Domain
{
    using DDDLite;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.Commands;
    using DDDLite.Validation;
    using System.Security.Cryptography;
    using System.Text;
    using Commands;

    public class Test : AggregateRoot
    {
        [Required]
        [MaxLength(16)]
        public string Code { get; set; }

        [Required]
        public int TestNum { get; set; }

        public Guid? EnabledById { get; set; }

        public User EnabledBy { get; set; }
    }
}