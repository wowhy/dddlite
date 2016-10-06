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
            var commandServiceTypes = types.Where(k => k.Name.EndsWith("CommandService") && k.GetTypeInfo().IsClass);
            var commandTypes = types.Where(k => k.Name.EndsWith("Command") && k.GetTypeInfo().IsClass);
            var validationTypes = types.Where(k => k.Name.EndsWith("Validator") && k.GetTypeInfo().IsClass);
            var queryServiceTypes = types.Where(k => k.Name.EndsWith("QueryService") && k.GetTypeInfo().IsClass);

            Console.WriteLine("注册命令服务");
            foreach (var type in commandServiceTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                var interfaceType = typeInfo.GetInterface("I" + type.Name);
                if (interfaceType == null)
                {
                    Console.WriteLine("未找到符合契约的接口!");
                    continue;
                }

                @this.AddScoped(interfaceType, type);
            }

            Console.WriteLine("注册命令");
            foreach (var type in commandTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                if (type.Name.EndsWith("CreateCommand"))
                {
                    var typeInfo = type.GetTypeInfo();
                    @this.AddScoped(typeInfo.GetInterface("ICreateCommand`1"), type);
                }
                else if (type.Name.EndsWith("UpdateCommand"))
                {
                    var typeInfo = type.GetTypeInfo();
                    @this.AddScoped(typeInfo.GetInterface("IUpdateCommand`1"), type);
                }
                else if (type.Name.EndsWith("DeleteCommand"))
                {
                    var typeInfo = type.GetTypeInfo();
                    @this.AddScoped(typeInfo.GetInterface("IDeleteCommand`1"), type);
                }
                else
                {
                    @this.AddScoped(type);
                }
            }

            Console.WriteLine("注册校验规则");
            foreach (var type in validationTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                @this.AddScoped(type);
            }

            Console.WriteLine("注册查询服务");
            foreach (var type in queryServiceTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                var interfaceType = typeInfo.GetInterface("I" + type.Name);
                if (interfaceType == null)
                {
                    Console.WriteLine("未找到符合契约的接口!");
                    continue;
                }

                @this.AddScoped(interfaceType, type);
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