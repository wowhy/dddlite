namespace DDDLite.Repositories.EF.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Xunit;
    using DDDLite.Specifications;

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

        [Fact]
        public void TestAdd()
        {
            var repository = context.GetRepository<Blog>();
            for (var i = 0; i < 1; i++)
            {
                var blog = Blog.Create();
                blog.Url = "http://localhost/blog/" + blog.Id;
                repository.Insert(blog);
            }

            context.Commit();
        }

        [Fact]
        public void TestGet()
        {
            var blog = (context.Context as SampleDbContext).Blogs.First();
            var repository = context.GetRepository<Blog>();
            var actual = repository.Get(blog.Id);

            Assert.NotNull(actual);
            Assert.Equal(blog.Id, actual.Id);
        }

        [Fact]
        public async Task TestGetAsync()
        {
            var blog = (context.Context as SampleDbContext).Blogs.First();
            var repository = context.GetRepository<Blog>();
            var actual = await repository.GetAsync(blog.Id);

            Assert.NotNull(actual);
            Assert.Equal(blog.Id, actual.Id);
        }

        [Fact]
        public void TestFind1()
        {
            var repository = context.GetRepository<Blog>();
            var blogs = repository.FindAll().ToList();

            Assert.NotNull(blogs);
            Assert.True(blogs.Count > 0);
        }

        [Fact]
        public void TestFind2()
        {
            var repository = context.GetRepository<Blog>();
            var blogs = repository.FindAll(Specification<Blog>.Eval(k => k.RowVersion == 0)).ToList();

            Assert.NotNull(blogs);
            Assert.True(blogs.Count > 0);
        }

        [Fact]
        public void TestFind3()
        {
            var repository = context.GetRepository<Blog>();
            var blogs = repository.FindAll(
                Specification<Blog>.Eval(k => k.RowVersion == 0),
                new SortSpecification<Blog>()
                {
                    { "Id", SortDirection.Desc }
                }).ToList();

            Assert.NotNull(blogs);
            Assert.True(blogs.Count > 0);
        }

        [Fact]
        public void TestConcurrencyCheck()
        {
            var repository = context.GetRepository<Blog>();
            var blog = repository.FindAll().First();

            try
            {
                blog.RowVersion = 100;
                repository.Update(blog);

                context.Commit();
                Assert.True(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void TestUpdate()
        {
            var repository = context.GetRepository<Blog>();
            var blog = repository.FindAll().First();

            try
            {
                blog.Url = "unit tests";
                repository.Update(blog);

                context.Commit();
                Assert.True(true);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }
    }
}
