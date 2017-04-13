namespace DDDLite.Actors
{
    using System;
    using System.Threading.Tasks;

    using Akka.Actor;

    using Repositories;
    using Commands;
    using Validation;

    public class AggregateRootActor<TAggregateRoot> : BaseActor
        where TAggregateRoot : class, IAggregateRoot, new()
    {
        private TAggregateRoot aggregate;
        private IDomainRepository<TAggregateRoot> repository;

        public AggregateRootActor(IDomainRepository<TAggregateRoot> repository)
        {
            this.repository = repository;
        }

        protected IDomainRepository<TAggregateRoot> Repository => this.repository;

        protected override void InitReceiveMessages()
        {
            ReceiveAsync<IAggregateRootCommand<TAggregateRoot>>(async (command) => await ProcessCommand(command));
        }

        protected virtual Task ProcessCommand(IAggregateRootCommand<TAggregateRoot> command)
        {
            return DoAction(async () =>
            {
                if (command is CreateCommand<TAggregateRoot>)
                {
                    await this.LoadAggregateRoot(command.AggregateRootId, true);
                }
                else
                {
                    await this.LoadAggregateRoot(command.AggregateRootId);
                }

                this.aggregate.HandleCommand(command);

                if (command is DeleteCommand<TAggregateRoot>)
                {
                    if (this.aggregate is ILogicalDelete)
                    {
                        await this.Save();
                    }
                    else
                    {
                        await this.Delete();
                    }
                }
                else
                {
                    await this.Save();
                }

                return this.aggregate;
            });
        }

        protected virtual async Task LoadAggregateRoot(Guid aggregateRootId, bool @new = false)
        {
            if (@new)
            {
                this.aggregate = new TAggregateRoot();
                this.aggregate.Id = aggregateRootId;
                return;
            }
            else
            {
                this.aggregate = await this.repository.GetByIdAsync(aggregateRootId);
                this.EnsureAggregateRootExists();
            }
        }

        protected void EnsureAggregateRootExists()
        {
            if (aggregate == null)
            {
                throw new CoreValidateException("当前操作数据不存在，该数据可能已经被删除，如有疑问请联系管理员！");
            }
        }

        protected virtual Task Save()
        {
            return this.Repository.SaveAsync(this.aggregate);
        }

        protected virtual Task Delete()
        {
            return this.Repository.DeleteAsync(this.aggregate);
        }
    }
}
