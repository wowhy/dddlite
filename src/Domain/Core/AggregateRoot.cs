﻿namespace Domain.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;

    public class AggregateRoot : Entity, IAggregateRoot
    {
        public Guid? CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [ConcurrencyCheck]
        public long RowVersion { get; set; }

        public AggregateRoot()
        {
        }
    }
}