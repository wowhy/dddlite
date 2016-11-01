﻿namespace DDDLite.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        private IDomainRepositoryContext context;
        private IDomainRepository<TAggregateRoot> repository;
        private Dictionary<Type, List<IValidator>> validators = new Dictionary<Type, List<IValidator>>();

        protected AggregateCommandHandler(IDomainRepositoryContext context)
        {
            this.context = context;
            this.repository = context.GetRepository<TAggregateRoot>();

            this.AddValidator(typeof(CreateCommand<TAggregateRoot>), new EntityValidator<TAggregateRoot>());
            this.AddValidator(typeof(UpdateCommand<TAggregateRoot>), new EntityValidator<TAggregateRoot>());
        }

        public IDomainRepositoryContext Context => this.context;

        public IDomainRepository<TAggregateRoot> Repository => this.repository;

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
            var entity = this.Repository.GetById(command.AggregateRootId);

            this.AssertEntityNotNull(entity);
            this.Validate(command);            

            entity.RowVersion = command.RowVersion;

            this.Repository.Delete(entity);
            this.Context.Commit();
        }

        public virtual void Handle(IUpdateCommand<TAggregateRoot> command)
        {
            var entity = this.Repository.GetById(command.AggregateRootId);

            this.AssertEntityNotNull(entity);
            this.Validate(command);
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
            if(entity == null)
            {
                throw new ValidationException("数据不存在，参数错误或当期数据已被删除！");
            }
        }
    }
}
