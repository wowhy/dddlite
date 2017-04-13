namespace DDDLite.Actors
{
    using System;

    using Akka.Actor;

    using Validation;
    using System.Threading.Tasks;

    public abstract class BaseActor : ReceiveActor
    {
        protected BaseActor()
        {
            this.InitReceiveMessages();
        }

        protected abstract void InitReceiveMessages();

        protected virtual async Task DoAction(Func<Task> action)
        {
            try
            {
                await action();
                TellSuccess();
            }
            catch (Exception ex)
            {
                TellFailure(ex);
            }
        }

        public virtual async Task DoAction<TData>(Func<Task<TData>> action)
        {
            try
            {
                var data = await action();
                TellSuccess(data);
            }
            catch (Exception ex)
            {
                TellFailure<TData>(ex);
            }
        }

        protected virtual void TellSuccess()
        {
            this.Sender.Tell(ActorResult.Success());
        }

        protected virtual void TellSuccess<TData>(TData data = default(TData))
        {
            this.Sender.Tell(ActorResult<TData>.Success(data));
        }

        protected virtual void TellFailure(Exception ex)
        {
            if (ex is CoreValidateException)
            {
                var result = ActorResult.Failure(ex.Message);
                result.StatusCode = 400;
                result.Details = ((CoreValidateException)ex).Details;
                this.Sender.Tell(result);
            }
            else
            {
                var result = ActorResult.Failure();
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                result.DebugMessage = ex.Message;
                result.StackTrace = ex.StackTrace;
                this.Sender.Tell(result);
            }
        }

        protected virtual void TellFailure<TData>(Exception ex)
        {
            if (ex is CoreValidateException)
            {
                var result = ActorResult<TData>.Failure(ex.Message);
                result.StatusCode = 400;
                result.Details = ((CoreValidateException)ex).Details;
                this.Sender.Tell(result);
            }
            else
            {
                var result = ActorResult<TData>.Failure();
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                result.DebugMessage = ex.Message;
                result.StackTrace = ex.StackTrace;
                this.Sender.Tell(result);
            }
        }
    }
}