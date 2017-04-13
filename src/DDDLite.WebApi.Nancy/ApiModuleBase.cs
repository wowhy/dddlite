using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Nancy;
using Newtonsoft.Json.Linq;

namespace DDDLite.WebApi.Nancy
{
    using DDDLite.Actors;
    using DDDLite.Commands;
    using DDDLite.Querying;
    using DDDLite.Validation;

    public abstract class ApiModuleBase<TAggregateRoot, TReadModel> : NancyModule
        where TAggregateRoot : class, IAggregateRoot, new()
        where TReadModel : class, new()
    {
        private IActorRef commandActor;
        private IActorRef queryActor;

        public ApiModuleBase(string modulePath, IActorRef commandActor, IActorRef queryActor) : base(modulePath)
        {
            this.commandActor = commandActor;
            this.queryActor = queryActor;

            this.Get("/{id:guid}", this.GetByIdAsync);
            this.Get("/", args => {
                Console.WriteLine(Request.Query.page);
                // Console.WriteLine(Request.Query.filters);
                return "Hello, World!";
            });
        }

        public IActorRef CommandActor => this.commandActor;
        public IActorRef QueryActor => this.queryActor;

        public virtual async Task<PagedResult<TReadModel>> PagedAsync(
            int page = 1,
            int limit = 10,
            List<Filter> filters = null,
            List<Sorter> sorters = null)
        {
            var result = await this.QueryActor.Ask<ActorResult<PagedResult<TReadModel>>>(new PagedInputForm(page, limit, filters, sorters));
            if (result.Successed)
            {
                return result.Result;
            }
            else
            {
                throw this.MakeException(result);
            }
        }

        public virtual async Task<object> GetByIdAsync(dynamic args)
        {
            var result = await this.QueryActor.Ask<ActorResult<TReadModel>>((Guid)args.Id);
            if (result.Successed)
            {
                if(result.Result == null)
                {
                    return Negotiate.WithStatusCode(404);
                }

                return result.Result;
            }
            else
            {
                throw this.MakeException(result);
            }
        }

        public async Task CreateAsync(JObject body)
        {
            var data = this.GetPostData(body);
            if (data.Id != default(Guid))
            {
                data.Id = SequentialGuid.Create();
            }

            var command = this.MakeCommand<CreateCommand<TAggregateRoot>>(data.Id, data);
            var result = await this.CommandActor.Ask<ActorResult>(command);

            if (!result.Successed)
            {
                throw MakeException(result);
            }
        }

        public async Task UpdateAsync(Guid id, JObject body)
        {
            var data = this.GetPutData(body);
            var command = this.MakeCommand<UpdateCommand<TAggregateRoot>>(data.Id, data);
            var result = await this.CommandActor.Ask<ActorResult>(command);

            if (!result.Successed)
            {
                throw MakeException(result);
            }
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var command = this.MakeCommand<DeleteCommand<TAggregateRoot>>(id, null);
            var result = await this.CommandActor.Ask<ActorResult>(command);
            if (!result.Successed)
            {
                throw MakeException(result);
            }
        }

        protected virtual TCommand MakeCommand<TCommand>(Guid aggregateId, TAggregateRoot aggregate)
            where TCommand : IAggregateRootCommand<TAggregateRoot>, new()
        {
            var command = new TCommand();

            command.AggregateRootId = aggregateId;
            command.AggregateRoot = aggregate;

            command.Timestamp = DateTime.Now;

            // if (this.User.Identity.IsAuthenticated)
            // {
            //     var claim = this.User.FindFirst(k => k.Type == ClaimTypes.NameIdentifier);
            //     command.OperatorId = Guid.Parse(claim.Value);
            // }

            return command;
        }

        protected virtual TAggregateRoot GetPostData(JObject body)
        {
            return body.ToObject<TAggregateRoot>();
        }

        protected virtual TAggregateRoot GetPutData(JObject body)
        {
            return body.ToObject<TAggregateRoot>();
        }

        protected Exception MakeException(ActorResult result)
        {
            if (result.StatusCode / 100 == 4)
            {
                return new CoreValidateException(result.Message);
            }
            else
            {
                return new CoreException(result.Message);
            }
        }
    }
}