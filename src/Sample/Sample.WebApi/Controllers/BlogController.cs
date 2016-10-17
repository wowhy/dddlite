namespace Sample.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using DDDLite.WebApi;
    using DDDLite.Messaging;

    using Core.Domain;
    using Core.Querying;

    [Route("api/blogs")]
    public class BlogController : RestfulApiController<Blog>
    {
        public BlogController(
            IServiceProvider serviceProvider,
            ICommandSender commandSender,
            IBlogQueryService queryService) :
            base(serviceProvider, commandSender, queryService)
        {
        }
    }
}