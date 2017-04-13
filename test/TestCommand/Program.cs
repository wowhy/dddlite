using System;
using Akka;
using Akka.Actor;

using DDDLite.Actors;
using DDDLite.Commands;

namespace TestCommand
{
    using Aggregates;
    using Akka.DI.AutoFac;
    using Autofac;
    using Commands;
    using DDDLite;
    using DDDLite.Repositories;
    using DDDLite.Repositories.EF;
    using Microsoft.EntityFrameworkCore;
    using Repositories;
    using System.Security.Cryptography;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using System.Threading;
    using DDDLite.Querying.EF;
    using DDDLite.Querying;

    public class Program
    {
        private static IContainer container;

        static void Main(string[] args)
        {
            RunProgram();
            Console.ReadKey();
        }

        public static async void RunProgram()
        {
            SetupDatabase();
            SetupAutoMapper();
            SetupAutoFac();

            var watch = new Stopwatch();
            watch.Start();

            var system = ActorSystem.Create("Test");
            var resolver = new AutoFacDependencyResolver(container, system);
            var tasks = new List<Task>();

            for (var i = 0; i < 1000; i++)
            {
                var actor = system.ActorOf(resolver.Create<CommandActor<User>>());
                var task = actor.Ask<ActorResult>(CreateUser());
                tasks.Add(task);
            }

            for (var i = 0; i < 1000; i++)
            {
                var actor = system.ActorOf(resolver.Create<QueryActor<User>>());
                var task = actor.Ask<ActorResult<User>>(Guid.Parse("39dd052b-6670-c239-036f-2869be837e5c"));
                tasks.Add(task);
            }

            for (var i = 0; i < 1000; i++)
            {
                var actor = system.ActorOf(resolver.Create<QueryActor<User>>());
                var task = actor.Ask<ActorResult<List<User>>>(new FindAllInputForm()
                {
                    Filters = new List<Filter>
                    {
                        new Filter("Name", "洪源", Filter.Operators.Contains),
                        new Filter("CreatedOn", DateTime.Now.ToString(), Filter.Operators.GreaterThan)
                    },
                    Sorters = new List<Sorter>
                    {
                        new Sorter("Id", SortDirection.Asc),
                        new Sorter("CreatedOn", SortDirection.Desc)
                    }
                });
                tasks.Add(task);
            }

            for (var i = 0; i < 1000; i++)
            {
                var actor = system.ActorOf(resolver.Create<QueryActor<User>>());
                var task = actor.Ask<ActorResult<PagedResult<User>>>(new PagedInputForm()
                {
                    Filters = new List<Filter>
                    {
                        new Filter("Name", "洪源", Filter.Operators.Contains)
                        {
                            OrConnectedFilters = new List<Filter>
                            {
                                new Filter("Code", "洪源", Filter.Operators.Contains)
                            }
                        },
                    },
                    Sorters = new List<Sorter>
                    {
                        new Sorter("Id", SortDirection.Asc),
                        new Sorter("CreatedOn", SortDirection.Desc)
                    }
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"takes: {watch.ElapsedMilliseconds}ms");
            Console.WriteLine("Error1: {0}", tasks.Where(k=>k is Task<ActorResult>).Select(k => k as Task<ActorResult>).Where(k => !k.Result.Successed).Count());
            Console.WriteLine("Error2: {0}", tasks.Where(k => k is Task<ActorResult<User>>).Select(k => k as Task<ActorResult<User>>).Where(k => !k.Result.Successed).Count());
            Console.WriteLine("Error3: {0}", tasks.Where(k => k is Task<ActorResult<List<User>>>).Select(k => k as Task<ActorResult<List<User>>>).Where(k => !k.Result.Successed).Count());
            Console.WriteLine("Error4: {0}", tasks.Where(k => k is Task<ActorResult<PagedResult<User>>>).Select(k => k as Task<ActorResult<PagedResult<User>>>).Where(k => !k.Result.Successed).Count());

            await system.Terminate();
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

        public static CreateCommand<User> CreateUser()
        {
            var salt = "qwe123";
            var password = "123456";
            password = String.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"{password}&{salt}")).Select(k => k.ToString("x")));
            var user = new User()
            {
                Code = "wowhy",
                Name = "洪源",
                Password = password,
                Salt = "qwe123"
            };
            user.NewIdentity();
            return new CreateCommand<User>()
            {
                AggregateRootId = user.Id,
                AggregateRoot = user
            };
        }
    }
}
