namespace DDDLite.WebApi
{
    using System;
    using System.Reflection;
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static void AddAssembly(this IServiceCollection @this, Assembly assembly)
        {
            var types = assembly.GetTypes();
            var commandHandlerTypes = types.Where(k => k.Name.EndsWith("CommandHandler") && k.GetTypeInfo().IsClass);
            var commandTypes = types.Where(k => k.Name.EndsWith("Command") && k.GetTypeInfo().IsClass);
            var validationTypes = types.Where(k => k.Name.EndsWith("Validator") && k.GetTypeInfo().IsClass);
            var queryServiceTypes = types.Where(k => k.Name.EndsWith("QueryService") && k.GetTypeInfo().IsClass);

            Console.WriteLine("Register CommandHandlers");
            foreach (var type in commandHandlerTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                var interfaceType = typeInfo.GetInterface("ICommandHandler`1");
                var interfaceTypes = typeInfo.GetInterfaces();

                if (type.Name.EndsWith("CreateCommandHandler"))
                {
                    @this.AddScoped(interfaceType, type);
                }
                else if (type.Name.EndsWith("UpdateCommandHandler"))
                {
                    @this.AddScoped(interfaceType, type);
                }
                else if (type.Name.EndsWith("DeleteCommandHandler"))
                {
                    @this.AddScoped(interfaceType, type);
                }
                else
                {
                    @this.AddScoped(interfaceType, type);
                }
            }
            Console.WriteLine();

            Console.WriteLine("Register Commands");
            foreach (var type in commandTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                if (type.Name.EndsWith("CreateCommand"))
                {
                    @this.AddScoped(typeInfo.GetInterface("ICreateCommand`1"), type);
                }
                else if (type.Name.EndsWith("UpdateCommand"))
                {
                    @this.AddScoped(typeInfo.GetInterface("IUpdateCommand`1"), type);
                }
                else if (type.Name.EndsWith("DeleteCommand"))
                {
                    @this.AddScoped(typeInfo.GetInterface("IDeleteCommand`1"), type);
                }
                else
                {
                    @this.AddScoped(type);
                }
            }
            Console.WriteLine();

            Console.WriteLine("Register Validations");
            foreach (var type in validationTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                @this.AddScoped(type);
            }
            Console.WriteLine();

            Console.WriteLine("Register Querying Services");
            foreach (var type in queryServiceTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                var interfaceType = typeInfo.GetInterface("I" + type.Name);
                if (interfaceType == null)
                {
                    @this.AddScoped(type);
                }
                else
                {
                    @this.AddScoped(interfaceType, type);
                }
            }
            Console.WriteLine();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(assembly);
            });
        }

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