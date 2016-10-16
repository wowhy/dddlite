namespace DDDLite.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Domain;
    using Repository;
    using Validation;

    public abstract class AggregateCommandHandler<TAggregateRoot>
        : ICommandHandler<ICreateCommand<TAggregateRoot>>
        , ICommandHandler<IUpdateCommand<TAggregateRoot>>
        , ICommandHandler<IDeleteCommand<TAggregateRoot>>

        where TAggregateRoot : class, IAggregateRoot, new()
    {
        private static IMapper mapper;

        static AggregateCommandHandler()
        {
            if (Mapper.Configuration.FindTypeMapFor<TAggregateRoot, TAggregateRoot>() != null)
            {
                mapper = Mapper.Instance;
            }
            else
            {
                IConfigurationProvider config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TAggregateRoot, TAggregateRoot>()
                       .ForMember(k => k.Id, k => k.Ignore())
                       .ForMember(k => k.CreatedById, k => k.Ignore())
                       .ForMember(k => k.CreatedOn, k => k.Ignore())
                       .ForMember(k => k.ModifiedById, k => k.Ignore())
                       .ForMember(k => k.ModifiedOn, k => k.Ignore())
                       .ForMember(k => k.RowVersion, k => k.Ignore());
                });
                mapper = new Mapper(config);
            }
        }

        private IDomainRepositoryContext context;
        private IDomainRepository<TAggregateRoot> repository;
        private Dictionary<Type, List<IValidator>> validators = new Dictionary<Type, List<IValidator>>();

        protected AggregateCommandHandler(IDomainRepositoryContext context)
        {
            this.context = context;
            this.repository = context.GetRepository<TAggregateRoot>();
        }

        public IDomainRepositoryContext Context => this.context;

        public IDomainRepository<TAggregateRoot> Repository => this.repository;

        public void AddValidator(Type commandType, IValidator validator)
        {
            List<IValidator> list;
            if (this.validators.ContainsKey(commandType))
            {
                list = this.validators[commandType];
            }
            else
            {
                list = new List<IValidator>();
                this.validators.Add(commandType, list);
            }

            list.Add(validator);
        }

        public List<IValidator> GetValidators(Type commandType)
        {
            return this.validators[commandType];
        }

        public virtual void Validate(IDeleteCommand<TAggregateRoot> command)
        {
            this.DoValidate(command);
        }

        public virtual void Validate(IUpdateCommand<TAggregateRoot> command)
        {
            this.DoValidate(command);
        }

        public virtual void Validate(ICreateCommand<TAggregateRoot> command)
        {
            this.DoValidate(command);
        }

        public virtual void Handle(IDeleteCommand<TAggregateRoot> command)
        {
            this.Validate(command);

            var entity = this.Repository.GetById(command.AggregateRootId);

            entity.RowVersion = command.RowVersion;

            this.Repository.Delete(entity);
            this.Context.Commit();
        }

        public virtual void Handle(IUpdateCommand<TAggregateRoot> command)
        {
            this.Validate(command);

            var entity = this.Repository.GetById(command.AggregateRootId);

            this.Map(command.AggregateRoot, entity);

            entity.ModifiedById = command.OperatorId;
            entity.ModifiedOn = command.Timestamp;
            entity.RowVersion = command.RowVersion;

            this.Repository.Update(entity);
            this.Context.Commit();
        }

        public virtual void Handle(ICreateCommand<TAggregateRoot> command)
        {
            this.Validate(command);

            var entity = new TAggregateRoot();

            this.Map(command.AggregateRoot, entity);

            entity.Id = command.AggregateRootId;
            entity.CreatedById = command.OperatorId;
            entity.CreatedOn = command.Timestamp;
            entity.RowVersion = command.RowVersion;

            this.Repository.Create(entity);
            this.Context.Commit();
        }

        public virtual void Map(TAggregateRoot source, TAggregateRoot destination)
        {
            mapper.Map<TAggregateRoot, TAggregateRoot>(source, destination);
        }

        private void DoValidate(ICommand command)
        {
            var validators = this.GetValidators(command.GetType());
            if (validators != null)
            {
                foreach (var validator in validators.OrderByDescending(k => k.Priority))
                {
                    validator.Validate(command);
                }
            }
        }
    }
}
