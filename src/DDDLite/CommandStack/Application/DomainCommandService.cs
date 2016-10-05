namespace DDDLite.CommandStack.Application
{
    using System;

    using AutoMapper;

    using Domain;
    using Commands;
    using Repository;

    public class DomainCommandService<TAggregateRoot> : IDomainCommandService<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly static IMapper mapper;
        static DomainCommandService()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<TAggregateRoot, TAggregateRoot>()
                   .ForMember(k => k.Id, opt => opt.Ignore())
                   .ForMember(k => k.CreatedById, opt => opt.Ignore())
                   .ForMember(k => k.CreatedOn, opt => opt.Ignore())
                   .ForMember(k => k.ModifiedById, opt => opt.Ignore())
                   .ForMember(k => k.ModifiedOn, opt => opt.Ignore())
                   .ForMember(k => k.RowVersion, opt => opt.Ignore())
            );
            mapper = config.CreateMapper();
        }



        private readonly IDomainRepositoryContext context;
        private readonly IDomainRepository<TAggregateRoot> repository;

        public IDomainRepositoryContext Context => this.context;

        public DomainCommandService(IDomainRepositoryContext context)
        {
            this.context = context;
            this.repository = context.GetRepository<TAggregateRoot>();
        }

        public void Handle(ICreateCommand<TAggregateRoot> cmd)
        {
            var entity = cmd.Data;

            entity.CreatedById = cmd.OperatorId;
            entity.CreatedOn = DateTime.Now;

            cmd.Validate();

            this.repository.Create(entity);
            this.context.Commit();
        }

        public void Handle(IUpdateCommand<TAggregateRoot> cmd)
        {
            var entity = this.repository.GetById(cmd.AggregateRootId);
            this.CheckEntityNotNull(entity);

            entity.ModifiedById = cmd.OperatorId;
            entity.ModifiedOn = DateTime.Now;
            entity.RowVersion = cmd.RowVersion;

            // map new value to orgin
            mapper.Map(cmd.Data, entity);

            cmd.Validate();

            this.repository.Update(entity);
            this.context.Commit();
        }

        public void Handle(IDeleteCommand<TAggregateRoot> cmd)
        {
            var entity = this.repository.GetById(cmd.AggregateRootId);
            this.CheckEntityNotNull(entity);

            entity.ModifiedById = cmd.OperatorId;
            entity.ModifiedOn = DateTime.Now;
            entity.RowVersion = cmd.RowVersion;

            cmd.Validate();

            this.repository.Delete(entity);
            this.context.Commit();
        }

        protected void CheckEntityNotNull(TAggregateRoot entity)
        {
            if (entity == null)
            {
                throw new Commands.Validation.ValidationException("当前指定的数据不存在！");
            }
        }
    }
}
