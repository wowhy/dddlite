namespace Sample.WebApi.Config
{
    using System;
    using System.Collections.Generic;
    using DDDLite.Commands;
    using DDDLite.Config;
    using DDDLite.Events;
    using Microsoft.Extensions.DependencyInjection;
    using Sample.Core.Commands;
    using Sample.Core.Entity;

    public class SampleRegister : Register
    {
        public SampleRegister(IServiceCollection services) : base(services)
        {
        }
    }
}
