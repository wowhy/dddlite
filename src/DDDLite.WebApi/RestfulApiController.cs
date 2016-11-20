namespace DDDLite.WebApi
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    using Commands;
    using Messaging;
    using Querying;
    using System.Security.Claims;

    public abstract class RestfulApiController<TAggregateRoot, TDTO> : Controller
        where TAggregateRoot : class, IAggregateRoot, new()
        where TDTO : class, new()
    {
        private readonly ICommandSender commandSender;
        private readonly IQueryService<TAggregateRoot> queryService;

        protected RestfulApiController(
            ICommandSender commandSender,
            IQueryService<TAggregateRoot> queryService)
        {
            this.commandSender = commandSender;
            this.queryService = queryService;
        }

        protected ICommandSender CommandSender => this.commandSender;

        protected IQueryService<TAggregateRoot> QueryService => this.queryService;

        [HttpGet]
        public virtual IActionResult Get([FromHeader(Name = "Query-Model")] string queryModel)
        {
            // this.HttpContext.Request.Headers["Domain-Model"];
            return this.Ok(this.queryService.Find<TDTO>());
        }

        [HttpGet("{id}")]
        public virtual IActionResult Get(Guid id, [FromHeader(Name = "Query-Model")] string queryModel)
        {
            var entity = this.queryService.GetById<TDTO>(id);
            if (entity != null)
            {
                return this.Ok(entity);
            }

            return this.NotFound();
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody] TAggregateRoot entity)
        {
            var cmd = new CreateCommand<TAggregateRoot>();

            entity.NewIdentity();

            cmd.AggregateRoot = entity;
            cmd.AggregateRootId = entity.Id;
            cmd.RowVersion = 0;
            cmd.OperatorId = this.User.GetUserId();

            this.commandSender.Publish(cmd);

            return this.NoContent();
        }

        [HttpPut("{id}")]
        public virtual IActionResult Put(Guid id, [FromBody] TAggregateRoot entity, [FromHeader(Name = "If-Match")] string ifMatch)
        {
            var cmd = new UpdateCommand<TAggregateRoot>();

            cmd.AggregateRootId = id;
            cmd.AggregateRoot = entity;
            cmd.RowVersion = long.Parse(ifMatch);
            cmd.OperatorId = this.User.GetUserId();

            this.commandSender.Publish(cmd);

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(Guid id, [FromHeader(Name = "If-Match")] string ifMatch)
        {
            var cmd = new DeleteCommand<TAggregateRoot>();

            cmd.AggregateRootId = id;
            cmd.RowVersion = long.Parse(ifMatch);
            cmd.OperatorId = this.User.GetUserId();

            this.commandSender.Publish(cmd);

            return this.NoContent();
        }
    }
}