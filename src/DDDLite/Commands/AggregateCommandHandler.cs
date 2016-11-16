namespace DDDLite.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Events;
    using Repository;
    using Validation;

    public abstract class AggregateCommandHandler<TAggregateRoot>
        : ICommandHandler<CreateCommand<TAggregateRoot>>
        , ICommandHandler<UpdateCommand<TAggregateRoot>>
        , ICommandHandler<DeleteCommand<TAggregateRoot>>

        where TAggregateRoot : class, IAggregateRoot, new()
    {
        private IDomainRepository<TAggregateRoot> repository;
        private Dictionary<Type, List<IValidator>> validators = new Dictionary<Type, List<IValidator>>();

        protected AggregateCommandHandler(IDomainRepository<TAggregateRoot> repository)
        {
            this.repository = repository;

            this.AddValidator(typeof(CreateCommand<TAggregateRoot>), new EntityValidator<TAggregateRoot>());
            this.AddValidator(typeof(UpdateCommand<TAggregateRoot>), new EntityValidator<TAggregateRoot>());
        }

        public IDomainRepository<TAggregateRoot> Repository => this.repository;

        public virtual void Validate(DeleteCommand<TAggregateRoot> command)
        {
            this.DoValidate(command);
        }

        public virtual void Validate(UpdateCommand<TAggregateRoot> command)
        {
            this.DoValidate(command);
        }

        public virtual void Validate(CreateCommand<TAggregateRoot> command)
        {
            this.DoValidate(command);
        }

        public virtual void Handle(DeleteCommand<TAggregateRoot> command)
        {
            var entity = this.Repository.GetById(command.AggregateRootId);

            this.AssertEntityNotNull(entity);
            this.Validate(command);

            entity.RowVersion = command.RowVersion;
            entity.RaiseEvent(new DeletedEvent<TAggregateRoot>());

            this.Repository.Save(entity);
        }

        public virtual void Handle(UpdateCommand<TAggregateRoot> command)
        {
            var entity = this.Repository.GetById(command.AggregateRootId);

            this.AssertEntityNotNull(entity);
            this.Validate(command);
            this.Map(command.AggregateRoot, entity);

            entity.ModifiedById = command.OperatorId;
            entity.ModifiedOn = command.Timestamp;
            entity.RowVersion = command.RowVersion;
            entity.RaiseEvent(new UpdatedEvent<TAggregateRoot>());

            this.Repository.Save(entity);
        }

        public virtual void Handle(CreateCommand<TAggregateRoot> command)
        {
            this.Validate(command);

            var entity = new TAggregateRoot();

            this.Map(command.AggregateRoot, entity);

            entity.Id = command.AggregateRootId;
            entity.CreatedById = command.OperatorId;
            entity.CreatedOn = command.Timestamp;
            entity.RowVersion = command.RowVersion;
            entity.RaiseEvent(new CreatedEvent<TAggregateRoot>());

            this.Repository.Save(entity);
        }

        public virtual void Map(TAggregateRoot source, TAggregateRoot destination)
        {
            Mapper.Map(source, destination);
        }

        protected void AddValidator<TCommand>(IValidator validator) where TCommand : class, ICommand
        {
            this.AddValidator(typeof(TCommand), validator);
        }

        protected void AddValidator(Type commandType, IValidator validator)
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

        protected List<IValidator> GetValidators(Type commandType)
        {
            var validators = default(List<IValidator>);
            this.validators.TryGetValue(commandType, out validators);
            return validators;
        }

        protected virtual void DoValidate(ICommand command)
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

        private void AssertEntityNotNull(TAggregateRoot entity)
        {
            if (entity == null)
            {
                throw new ValidationException("数据不存在，参数错误或当期数据已被删除！");
            }
        }
    }
}
