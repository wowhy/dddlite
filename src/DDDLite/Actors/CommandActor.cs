namespace DDDLite.Actors
{
    using System;
    using System.Threading.Tasks;

    using Akka.Actor;

    using Commands;
    using Repositories;

    public class CommandActor<TAggregateRoot> : BaseActor
        where TAggregateRoot : class, IAggregateRoot, new()
    {
        private IDomainRepository<TAggregateRoot> repository;

        public CommandActor(IDomainRepository<TAggregateRoot> repository) : base()
        {
            this.repository = repository;
        }

        protected IDomainRepository<TAggregateRoot> Repository => this.repository;

        protected override void InitReceiveMessages()
        {
            ReceiveAnyAsync(this.PreProcessMessage);
        }

        protected virtual Task PreProcessMessage(object message)
        {
            if (message is ICommand)
            {
                return this.ProcessCommand((ICommand)message);
            }

            // TODO: 未知命令格式，记录日志
            return Task.CompletedTask;
        }

        protected async virtual Task ProcessCommand(ICommand command)
        {
            var success = await this.Validate(command);
            if (!success)
            {
                return;
            }

            if (command is IAggregateRootCommand<TAggregateRoot>)
            {
                this.ForwardAggregateRootCommand(
                    (IAggregateRootCommand<TAggregateRoot>)command,
                    Props.Create(typeof(AggregateRootActor<TAggregateRoot>), this.repository));
            }
            else
            {
                // 未知命令
            }
        }

        protected async Task<bool> Validate(ICommand command)
        {
            var result = await this.OnValidate(command);
            if (!result.Successed)
            {
                this.Sender.Tell(result);
            }

            return result.Successed;
        }

        protected virtual void ForwardAggregateRootCommand(IAggregateRootCommand<TAggregateRoot> command, Props props)
        {
            var aggregateActor = Context.Child(command.AggregateRootId.ToString());
            if (aggregateActor.Equals(ActorRefs.Nobody))
            {
                aggregateActor = Context.ActorOf(props, command.AggregateRootId.ToString());
            }

            aggregateActor.Forward(command);
        }

        protected virtual Task<ActorResult> AskValidateResult(ICommand command, Props props)
        {
            var validateActor = Context.Child("validator");
            if (validateActor.Equals(ActorRefs.Nobody))
            {
                validateActor = Context.ActorOf(props, "validator");
            }

            return validateActor.Ask<ActorResult>(command);
        }

        protected virtual Task<ActorResult> OnValidate(ICommand command)
        {
            return Task.FromResult(ActorResult.Success());
        }
    }
}