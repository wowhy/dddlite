namespace TestWebApi
{
    using Akka.Actor;
    using Akka.DI.AutoFac;
    using Autofac;
    using DDDLite.Actors;
    using DDDLite.Querying;
    using DDDLite.Querying.EF;
    using DDDLite.Repositories;
    using DDDLite.Repositories.EF;
    using DDDLite.WebApi.Nancy;
    using Microsoft.EntityFrameworkCore;
    using Nancy.Configuration;
    using Nancy;
    using TestWebApi.Aggregates;
    using TestWebApi.Repositories;

    public class TestBootstrapper : ApiBootstrapper
    {
        private static IContainer container;
        public static IActorRef UserCommandActor { get; set; }
        public static IActorRef UserQueryActor { get; set; }

        public override void Configure(INancyEnvironment environment)
        {
            environment.Tracing(enabled: false, displayErrorTraces: true);

            SetupAutoFac();
            SetupAutoMapper();
            SetupDatabase();

            var system = ActorSystem.Create("Test");
            var resolver = new AutoFacDependencyResolver(container, system);

            UserCommandActor = system.ActorOf(resolver.Create<CommandActor<User>>());
            UserQueryActor = system.ActorOf(resolver.Create<QueryActor<User>>());
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