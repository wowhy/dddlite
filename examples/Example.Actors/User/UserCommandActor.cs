namespace Example.Actors
{
    using Core.Domain;
    using Akka.Actor;
    using DDDLite.Actors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.Repositories;
    using DDDLite.Commands;
    using Core.Commands;
    using DDDLite.Querying;
    using DDDLite.Validation;

    public class UserCommandActor : CommandActor<User>
    {
        public UserCommandActor(IDomainRepository<User> repository) : base(repository)
        {
        }

        protected override Task<ActorResult> OnValidate(ICommand command)
        {
            if (command is CreateCommand<User>)
            {
                return this.AskValidateResult(command, ExampleSystem.Context.ActorResolver.Create(typeof(UserValidateActor)));
            }

            return base.OnValidate(command);
        }

        protected override Task ProcessCommand(ICommand message)
        {
            if (message is ChangePasswordCommand)
            {
                //return DoAction(async () =>
                //{
                //    var command = (ChangePasswordCommand)message;
                //    var entity = await this.Repository.GetByIdAsync(command.UserId);
                //    if (entity != null)
                //    {
                //        entity.Handle(command);
                //        await this.Repository.SaveAsync(entity);
                //    }
                //    else
                //    {
                //        throw new CoreValidateException("参数不正确，找不到当前指定用户！");
                //    }
                //});
            }

            return base.ProcessCommand(message);
        }
    }
}
