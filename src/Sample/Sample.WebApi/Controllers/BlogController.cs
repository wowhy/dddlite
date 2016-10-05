namespace Sample.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using DDDLite.WebApi;

    using Sample.Core.Domain;
    using Sample.Core.CommandStack.Application;
    using Sample.Core.QueryStack.Application;

    public class BlogDTO
    {
        public List<Post> Posts { get; set; }
    }

    [Route("api/blogs")]
    public class BlogController : RestfulApiController<Blog, Blog>
    {
        public BlogController(
            IServiceProvider serviceProvider,
            IBlogCommandService commandService,
            IBlogQueryService queryService) :
            base(serviceProvider, commandService, queryService)
        {
        }
    }
}