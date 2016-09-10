namespace Repository.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Specifications;

    [TestClass]
    public class TestRepository
    {
        private SampleRepositoryContext context = new SampleRepositoryContext();

        public TestRepository()
        {
            var serviceProvider = context.Context.GetInfrastructure<IServiceProvider>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(new MyLoggerProvider());
        }

        [TestMethod]
        public void TestAdd()
        {
            var repository = context.GetRepository<Guid, Blog>();
            for (var i = 0; i < 1; i++)
            {
                var blog = Entity.Create<Blog>();
                blog.Url = "http://localhost/blog/" + blog.Id;
                repository.Add(blog);
            }

            context.Commit();
        }

        [TestMethod]
        public void TestGet()
        {
            var blog = (context.Context as SampleDbContext).Blogs.First();
            var repository = context.GetRepository<Guid, Blog>();
            var actual = repository.Get(blog.Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(blog.Id, actual.Id);
        }

        [TestMethod]
        public async Task TestGetAsync()
        {
            var blog = (context.Context as SampleDbContext).Blogs.First();
            var repository = context.GetRepository<Guid, Blog>();
            var actual = await repository.GetAsync(blog.Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(blog.Id, actual.Id);
        }

        [TestMethod]
        public void TestFind1()
        {
            var repository = context.GetRepository<Guid, Blog>();
            var blogs = repository.FindAll().ToList();

            Assert.IsNotNull(blogs);
            Assert.IsTrue(blogs.Count > 0);
        }

        [TestMethod]
        public void TestFind2()
        {
            var repository = context.GetRepository<Guid, Blog>();
            var blogs = repository.FindAll(Specification<Blog>.Eval(k => k.RowVersion == 0)).ToList();

            Assert.IsNotNull(blogs);
            Assert.IsTrue(blogs.Count > 0);
        }

        [TestMethod]
        public void TestFind3()
        {
            var repository = context.GetRepository<Guid, Blog>();
            var blogs = repository.FindAll(
                Specification<Blog>.Eval(k => k.RowVersion == 0),
                new SortSpecification<Guid, Blog>()
                {
                    { "Id", SortDirection.Desc }
                }).ToList();

            Assert.IsNotNull(blogs);
            Assert.IsTrue(blogs.Count > 0);
        }
    }
}
