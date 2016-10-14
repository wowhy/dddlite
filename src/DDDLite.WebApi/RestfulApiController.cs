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

    public abstract class RestfulApiController<TAggregateRoot, TDTO> : Controller
        where TAggregateRoot : class, IAggregateRoot
        where TDTO : class
    {
        private static readonly IConfigurationProvider configuration;

        private static readonly IMapper mapper;

        static RestfulApiController()
        {
            var typeMapper = Mapper.Configuration.FindTypeMapFor(typeof(TAggregateRoot), typeof(TDTO));
            if (typeMapper == null)
            {
                configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TAggregateRoot, TDTO>();
                });
            }
            else
            {
                mapper = Mapper.Instance;
                configuration = Mapper.Configuration;
            }
        }

        private readonly IServiceProvider serviceProvider;
        private readonly ICommandSender commandSender;

        protected RestfulApiController(IServiceProvider serviceProvider, ICommandSender commandSender)
        {
            this.serviceProvider = serviceProvider;
            this.commandSender = commandSender;
        }

        protected IServiceProvider ServiceProvider => this.serviceProvider;

        protected ICommandSender CommandSender => this.commandSender;

        [HttpGet]
        public virtual IQueryable<TDTO> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public virtual TDTO Get(Guid id)
        {
            throw new NotImplementedException();
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