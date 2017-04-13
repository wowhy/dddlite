using Akka.Actor;
using Akka.DI.AutoFac;
using Autofac;
using DDDLite.Actors;
using DDDLite.Querying;
using DDDLite.Querying.EF;
using DDDLite.Repositories;
using DDDLite.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCommand.Aggregates;
using TestCommand.Repositories;

namespace TestWebApi.Mvc
{
    public class TestModuleSystem : ModuleSystem
    {
        private static IContainer container;

        public TestModuleSystem() : base(null, null, null)
        {
        }

        public override void Initialize()
        {
            SetupAutoFac();
            SetupAutoMapper();
            SetupDatabase();
            
            var system = ActorSystem.Create("Test");
            var resolver = new AutoFacDependencyResolver(container, system);

            this.ActorFactory = system;
            this.ActorResolver = resolver;

            system.ActorOf(resolver.Create<CommandActor<User>>(), "user/command");
            system.ActorOf(resolver.Create<QueryActor<User>>(), "user/query");
        }

        public static void SetupAutoFac()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<QueryActor<User>>()
                   .AsSelf();

            builder.RegisterType<CommandActor<User>>()
                   .AsSelf();

            builder.RegisterType<DemoDbContext>()
                   .AsSelf()
                   .As<DbContext>();

            builder.RegisterType<EFDomainRepository<User>>()
                   .AsSelf()
                   .As<IDomainRepository<User>>();

            builder.RegisterType<EFQueryService<User>>()
                   .AsSelf()
                   .As<IQueryService<User>>();

            container = builder.Build();
        }

        public static void SetupAutoMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, User>()
                   .ForMember(k => k.CreatedById, opt => opt.Ignore())
                   .ForMember(k => k.CreatedOn, opt => opt.Ignore())
                   .ForMember(k => k.ModifiedById, opt => opt.Ignore())
                   .ForMember(k => k.ModifiedOn, opt => opt.Ignore())
                   .ForMember(k => k.RowVersion, opt => opt.Ignore());
            });
        }

        public static void SetupDatabase()
        {
            new DemoDbContext().Database.EnsureCreated();
        }
    }
}
