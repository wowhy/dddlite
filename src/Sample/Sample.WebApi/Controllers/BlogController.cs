namespace Sample.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using DDDLite.WebApi;
    using DDDLite.Messaging;

    using Sample.Core.Domain;

    public class BlogDTO
    {
        public List<Post> Posts { get; set; }
    }

    [Route("api/blogs")]
    public class BlogController : RestfulApiController<Blog, BlogDTO>
    {
        public BlogController(
            IServiceProvider serviceProvider,
            ICommandSender commandSender) :
            base(serviceProvider, commandSender)
        {
        }
    }
}