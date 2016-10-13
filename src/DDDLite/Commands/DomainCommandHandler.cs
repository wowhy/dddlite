namespace DDDLite.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain;
    using Validation;

    public abstract class DomainCommandHandler<TCommand, TAggregateRoot> : IDomainCommandHandler<TCommand, TAggregateRoot>
        where TCommand : IDomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private static IMapper mapper;

        static DomainCommandHandler()
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
        private List<IValidator> validators = new List<IValidator>();

        public IDomainRepositoryContext Context => this.context;

        public IDomainRepository<TAggregateRoot> Repository => this.repository;

        public ICollection<IValidator> Validators => this.validators;

        protected DomainCommandHandler(IDomainRepositoryContext context) : this(context, null)
        {
        }

        protected DomainCommandHandler(IDomainRepositoryContext context, params IValidator[] validators)
        {
            this.context = context;
            this.repository = context.GetRepository<TAggregateRoot>();

            if (validators != null)
            {
                this.validators.AddRange(validators);
            }
        }

        public virtual void Validate(TCommand command)
        {
            foreach (var validator in this.Validators.OrderByDescending(k => k.Priority))
            {
                validator.Validate(command);
            }
        }

        public virtual Task HandleAsync(TCommand command)
        {
            this.Validate(command);
            return this.DoHandleAsync(command);
        }

        public virtual void Map(TAggregateRoot source, TAggregateRoot destination)
        {
            mapper.Map<TAggregateRoot, TAggregateRoot>(source, destination);
        }

        public abstract Task DoHandleAsync(TCommand command);
    }
}
