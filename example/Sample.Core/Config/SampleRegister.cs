namespace Sample.Core.Config
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using DDDLite.Commands;
    using DDDLite.Config;
    using DDDLite.Events;
    using Entity;
    using Microsoft.Extensions.DependencyInjection;

    public class SampleRegister : Register
    {
        public SampleRegister(IServiceCollection services) : base(services)
        {
        }

        protected override IDictionary<Type, Func<IServiceProvider, ICommandHandler>> CommandHandlerRoutes => new Dictionary<Type, Func<IServiceProvider, ICommandHandler>>()
        {
            { typeof(CreateCommand<Blog>), provider => provider.GetService<BlogCommandHandler>() },
            { typeof(UpdateCommand<Blog>), provider => provider.GetService<BlogCommandHandler>() },
            { typeof(DeleteCommand<Blog>), provider => provider.GetService<BlogCommandHandler>() }
        };
        protected override IDictionary<Type, Func<IServiceProvider, IEventHandler>> EventHandlerRoutes => new Dictionary<Type, Func<IServiceProvider, IEventHandler>>()
        {
        };
    }
}
