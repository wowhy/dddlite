namespace Sample.WebApi.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using DDDLite.WebApi;
    using DDDLite.Messaging;

    using Core.Entity;
    using Core.Querying;
    using Core.DTO;

    [Route("api/posts")]
    public class PostController : RestfulApiController<Post, PostDTO>
    {
        public PostController(
            ICommandSender commandSender,
            IPostQueryService queryService) :
            base(commandSender, queryService)
        {
        }
    }
}