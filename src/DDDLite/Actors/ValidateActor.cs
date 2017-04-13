using System;
using System.Threading.Tasks;
using Akka.Actor;
using DDDLite.Commands;

namespace DDDLite.Actors
{
    public class ValidateActor : BaseActor
    {
        public ValidateActor()
        {
        }

        protected virtual void ValidateAsync<TCommand>(Func<TCommand, Task<ActorResult>> validate)
        {
            this.ReceiveAsync<TCommand>(async (command) =>
            {
                try
                {
                    var result = await validate(command);
                    if (!result.Successed)
                    {
                        result.StatusCode = 400;
                    }

                    this.Sender.Tell(result);
                }
                catch (Exception ex)
                {
                    TellFailure(ex);
                }
            });
        }

        protected override void InitReceiveMessages()
        {
            this.ReceiveAny((m) => this.TellSuccess());
        }

        protected virtual void TellValidateFailed(string message, string[] detail = null)
        {
            var result = ActorResult.Failure(message);
            result.StatusCode = 400;
            result.Details = detail;
            this.Sender.Tell(result);
        }
    }
}
