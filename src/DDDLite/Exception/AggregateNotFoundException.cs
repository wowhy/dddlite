﻿namespace DDDLite.Exception
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AggregateNotFoundException : CoreException
    {
        public AggregateNotFoundException(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; private set; }
    }
}
