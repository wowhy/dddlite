namespace DDDLite.Config
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    using Commands;

    public class Register
    {
        private readonly IServiceCollection services;

        public Register(IServiceCollection services)
        {
            this.services = services;
        }

        public void RegisterCommands(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var commandTypes = types.Where(k => k.Name.EndsWith("Command") && k.GetTypeInfo().IsClass && !k.GetTypeInfo().IsAbstract);

            Console.WriteLine("Register Commands");
            foreach (var type in commandTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                if (type.Name.EndsWith("CreateCommand"))
                {
                    this.services.AddScoped(typeInfo.GetInterface("ICreateCommand`1"), type);
                }
                else if (type.Name.EndsWith("UpdateCommand"))
                {
                    this.services.AddScoped(typeInfo.GetInterface("IUpdateCommand`1"), type);
                }
                else if (type.Name.EndsWith("DeleteCommand"))
                {
                    this.services.AddScoped(typeInfo.GetInterface("IDeleteCommand`1"), type);
                }
                else
                {
                    this.services.AddScoped(type);
                }
            }
            Console.WriteLine();
        }

        public void RegisterValidators(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var validationTypes = types.Where(k => k.Name.EndsWith("Validator") && k.GetTypeInfo().IsClass && !k.GetTypeInfo().IsAbstract);

            Console.WriteLine("Register Validators");
            foreach (var type in validationTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                this.services.AddScoped(type);
            }
            Console.WriteLine();
        }

        public void RegisterCommandHandlers(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var commandHandlerTypes = types.Where(k => k.Name.EndsWith("CommandHandler") && k.GetTypeInfo().IsClass);

            Console.WriteLine("Register CommandHandlers");
            foreach (var type in commandHandlerTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                var interfaceTypes = typeInfo.GetInterfaces().Where(k => k.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                foreach (var interfaceType in interfaceTypes)
                {
                    this.services.AddScoped(interfaceType, type);
                }
            }
            Console.WriteLine();
        }

        public void RegisterQueryServices(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var queryServiceTypes = types.Where(k => k.Name.EndsWith("QueryService") && k.GetTypeInfo().IsClass && !k.GetTypeInfo().IsAbstract);

            Console.WriteLine("Register Querying Services");
            foreach (var type in queryServiceTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                var interfaceType = typeInfo.GetInterface("I" + type.Name);
                if (interfaceType == null)
                {
                    this.services.AddScoped(type);
                }
                else
                {
                    this.services.AddScoped(interfaceType, type);
                }
            }
            Console.WriteLine();
        }

        public void RegisterAutoMapper(params Assembly[] assemblies)
        {
            AutoMapper.Mapper.Initialize(cfg => 
            {
                cfg.AddProfiles(assemblies);
            });
        }
    }
}
