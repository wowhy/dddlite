namespace DDDLite.WebApi
{
    using System;
    using System.Reflection;
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;
    using Commands;

    public static class Extensions
    {
        public static void AddRepositoryContext<TInterface, TRepositoryContext>(this IServiceCollection @this)
            where TInterface : class
            where TRepositoryContext : class, TInterface
        {
            @this.AddScoped<TInterface, TRepositoryContext>();
        }

        public static void AddRepository<TContext, TRepository>(this IServiceCollection @this, Func<TContext, TRepository> func)
            where TContext : class
            where TRepository : class
        {
            @this.AddScoped<TRepository>(provider => func(provider.GetService<TContext>()));
        }
    }
}