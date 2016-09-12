namespace Repository.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core;
    using Microsoft.EntityFrameworkCore;
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

            context.Context.Database.EnsureCreated();
        }

        [TestMethod]
        public void TestAdd()
        {
            var repository = context.GetRepository<Blog>();
            for (var i = 0; i < 1; i++)
            {
                var blog = Blog.Create();
                blog.Url = "http://localhost/blog/" + blog.Id;
                repository.Add(blog);
            }

            context.Commit();
        }

        [TestMethod]
        public void TestGet()
        {
            var blog = (context.Context as SampleDbContext).Blogs.First();
            var repository = context.GetRepository<Blog>();
            var actual = repository.Get(blog.Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(blog.Id, actual.Id);
        }

        [TestMethod]
        public async Task TestGetAsync()
        {
            var blog = (context.Context as SampleDbContext).Blogs.First();
            var repository = context.GetRepository<Blog>();
            var actual = await repository.GetAsync(blog.Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(blog.Id, actual.Id);
        }

        [TestMethod]
        public void TestFind1()
        {
            var repository = context.GetRepository<Blog>();
            var blogs = repository.FindAll().ToList();

            Assert.IsNotNull(blogs);
            Assert.IsTrue(blogs.Count > 0);
        }

        [TestMethod]
        public void TestFind2()
        {
            var repository = context.GetRepository<Blog>();
            var blogs = repository.FindAll(Specification<Blog>.Eval(k => k.RowVersion == 0)).ToList();

            Assert.IsNotNull(blogs);
            Assert.IsTrue(blogs.Count > 0);
        }

        [TestMethod]
        public void TestFind3()
        {
            var repository = context.GetRepository<Blog>();
            var blogs = repository.FindAll(
                Specification<Blog>.Eval(k => k.RowVersion == 0),
                new SortSpecification<Blog>()
                {
                    { "Id", SortDirection.Desc }
                }).ToList();

            Assert.IsNotNull(blogs);
            Assert.IsTrue(blogs.Count > 0);
        }

        [TestMethod]
        public void TestConcurrencyCheck()
        {
            var repository = context.GetRepository<Blog>();
            var blog = repository.FindAll().First();

            try
            {
                blog.RowVersion = 100;
                repository.Update(blog);

                context.Commit();
                Assert.IsTrue(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            var repository = context.GetRepository<Blog>();
            var blog = repository.FindAll().First();

            try
            {
                blog.Url = "unit tests";
                repository.Update(blog);

                context.Commit();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }
    }
}
