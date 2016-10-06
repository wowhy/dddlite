namespace DDDLite.WebApi
{
    using System;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    using Domain;
    using Commands;
    using CommandStack.Application;
    using QueryStack.Application;
    using Specifications;

    public abstract class RestfulApiController<TAggregateRoot, TDTO> : Controller
        where TAggregateRoot : class, IAggregateRoot
        where TDTO : class
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDomainCommandService<TAggregateRoot> commandService;
        private readonly IQueryService<TAggregateRoot> queryService;

        private readonly IConfigurationProvider configuration;

        protected RestfulApiController(
            IServiceProvider serviceProvider,
            IDomainCommandService<TAggregateRoot> commandService,
            IQueryService<TAggregateRoot> queryService)
        {
            this.serviceProvider = serviceProvider;
            this.commandService = commandService;
            this.queryService = queryService;

            var mapper = Mapper.Configuration.FindTypeMapFor(typeof(TAggregateRoot), typeof(TDTO));
            if (mapper == null)
            {
                this.configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TAggregateRoot, TDTO>();
                });
            }
            else
            {
                this.configuration = Mapper.Configuration;
            }
        }

        protected IServiceProvider ServiceProvider => this.serviceProvider;

        protected IDomainCommandService<TAggregateRoot> CommandService => this.commandService;

        protected IQueryService<TAggregateRoot> QueryService => this.queryService;

        [HttpGet]
        public virtual IQueryable<TDTO> Get()
        {
            // TODO: 实现查询接口
            return this.queryService.FindAll().ProjectToQueryable<TDTO>(this.configuration);
        }

        [HttpGet("{id}")]
        public virtual TDTO Get(Guid id)
        {
            // TODO: 实现查询接口
            return this.queryService.FindAll(Specification<TAggregateRoot>.Eval(k => k.Id == id)).ProjectToFirstOrDefault<TDTO>();
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody] TAggregateRoot entity)
        {
            var cmd = this.serviceProvider.GetService<ICreateCommand<TAggregateRoot>>();

            entity.NewIdentity();
            cmd.Data = entity;

            this.commandService.Handle(cmd);
            return this.Created("", entity);
        }

        [HttpPut("{id}")]
        public virtual IActionResult Put(Guid id, [FromBody] TAggregateRoot entity, [FromHeader(Name = "If-Match")] string ifMatch)
        {
            var cmd = this.serviceProvider.GetService<IUpdateCommand<TAggregateRoot>>();

            cmd.AggregateRootId = id;
            cmd.Data = entity;
            cmd.RowVersion = long.Parse(ifMatch);

            this.commandService.Handle(cmd);
            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(Guid id, [FromHeader(Name = "If-Match")] string ifMatch)
        {
            var cmd = this.serviceProvider.GetService<IDeleteCommand<TAggregateRoot>>();

            cmd.AggregateRootId = id;
            cmd.RowVersion = long.Parse(ifMatch);

            this.commandService.Handle(cmd);
            return this.NoContent();
        }
    }
}