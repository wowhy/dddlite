namespace DDDLite.WebApi
{
    using System;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    using Commands;
    using Domain;
    using Messaging;
    using Querying;

    public abstract class RestfulApiController<TAggregateRoot> : Controller
        where TAggregateRoot : class, IAggregateRoot, new()
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ICommandSender commandSender;
        private readonly IQueryService<TAggregateRoot> queryService;

        protected RestfulApiController(
            IServiceProvider serviceProvider,
            ICommandSender commandSender,
            IQueryService<TAggregateRoot> queryService)
        {
            this.serviceProvider = serviceProvider;
            this.commandSender = commandSender;
            this.queryService = queryService;
        }

        protected IServiceProvider ServiceProvider => this.serviceProvider;

        protected ICommandSender CommandSender => this.commandSender;

        protected IQueryService<TAggregateRoot> QueryService => this.queryService;

        [HttpGet]
        public virtual IQueryable<TAggregateRoot> Get()
        {
            return this.queryService.FindAll();
        }

        [HttpGet("{id}")]
        public virtual IActionResult Get(Guid id)
        {
            var entity = this.queryService.GetById(id);
            if (entity != null)
            {
                return this.Ok(entity);
            }

            return this.NotFound();
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody] TAggregateRoot entity)
        {
            var cmd = this.serviceProvider.GetService<ICreateCommand<TAggregateRoot>>();

            entity.NewIdentity();

            cmd.AggregateRoot = entity;
            cmd.AggregateRootId = entity.Id;
            cmd.RowVersion = 0;
            // cmd.OperatorId = createdById;

            this.commandSender.Publish(cmd);

            return this.NoContent();
        }

        [HttpPut("{id}")]
        public virtual IActionResult Put(Guid id, [FromBody] TAggregateRoot entity, [FromHeader(Name = "If-Match")] string ifMatch)
        {
            var cmd = this.serviceProvider.GetService<IUpdateCommand<TAggregateRoot>>();

            cmd.AggregateRootId = id;
            cmd.AggregateRoot = entity;
            cmd.RowVersion = long.Parse(ifMatch);
            // cmd.OperatorId = modifiedById;

            this.commandSender.Publish(cmd);

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(Guid id, [FromHeader(Name = "If-Match")] string ifMatch)
        {
            var cmd = this.serviceProvider.GetService<IDeleteCommand<TAggregateRoot>>();

            cmd.AggregateRootId = id;
            cmd.RowVersion = long.Parse(ifMatch);
            // cmd.OperatorId = deletedById;

            this.commandSender.Publish(cmd);

            return this.NoContent();
        }
    }
}