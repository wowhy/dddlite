namespace Sample.WebApi.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using DDDLite.WebApi;
    using DDDLite.Messaging;

    using Sample.Core.Domain;

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
            ICommandSender commandSender) :
            base(serviceProvider, commandSender)
        {
        }
    }
}