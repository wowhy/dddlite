namespace Sample.WebApi.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using DDDLite.WebApi;
    using DDDLite.Messaging;

    using Core.Domain;
    using Core.Querying;

    public class PostDTO
    {
        public string Title { get; set; }

        public string Blog_Title { get; set; }
    }

    [Route("api/posts")]
    public class PostController : RestfulApiController<Post>
    {
        public PostController(
            IServiceProvider serviceProvider,
            ICommandSender commandSender,
            IPostQueryService queryService) :
            base(serviceProvider, commandSender, queryService)
        {
        }
    }
}