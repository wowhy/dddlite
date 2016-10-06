namespace Sample.WebApi.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using DDDLite.WebApi;

    using Sample.Core.Domain;
    using Sample.Core.CommandStack.Application;
    using Sample.Core.QueryStack.Application;

    public class PostDTO
    {
        public string Title { get; set; }

        public string Blog_Title { get; set; }
    }

    [Route("api/posts")]
    public class PostController : RestfulApiController<Post, PostDTO>
    {
        public PostController(
            IServiceProvider serviceProvider,
            IPostCommandService commandService,
            IPostQueryService queryService) :
            base(serviceProvider, commandService, queryService)
        {
        }
    }
}