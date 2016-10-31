namespace Sample.WebApi.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using DDDLite.WebApi;
    using DDDLite.Messaging;

    using Core.Domain;
    using Core.Querying;
    using Core.DTO;

    [Route("api/posts")]
    public class PostController : RestfulApiController<Post, PostDTO>
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