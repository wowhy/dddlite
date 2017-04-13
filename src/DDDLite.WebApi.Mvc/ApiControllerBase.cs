namespace DDDLite.WebApi.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Security.Claims;

    using Akka.Actor;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;

    using Actors;
    using Commands;
    using Validation;
    using Querying;
    using Microsoft.Extensions.Primitives;
    using Microsoft.AspNetCore.Authorization;
    using Newtonsoft.Json;

    [Authorize]
    public class ApiControllerBase<TAggregateRoot, TReadModel> : Controller
        where TAggregateRoot : class, IAggregateRoot, new()
        where TReadModel : class, new()
    {
        private ICanTell commandActor;
        private ICanTell queryActor;

        public ApiControllerBase(ICanTell commandActor, ICanTell queryActor)
        {
            this.commandActor = commandActor;
            this.queryActor = queryActor;
        }

        public ICanTell CommandActor => this.commandActor;
        public ICanTell QueryActor => this.queryActor;

        [HttpGet]
        public virtual async Task<PagedResult<TReadModel>> Get(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery(Name = "filters")] string rawFilters = null,
            [FromQuery(Name = "sorters")] string rawSorters = null,
            [FromQuery(Name = "eagerLoadings")] string rawEagerLoadings = null)
        {
            var filters = default(List<Filter>);
            var sorters = default(List<Sorter>);
            var eagerLoadings = default(List<string>);

            if (!string.IsNullOrWhiteSpace(rawFilters))
            {
                filters = JsonConvert.DeserializeObject<List<Filter>>(rawFilters);
            }

            if (!string.IsNullOrWhiteSpace(rawSorters))
            {
                sorters = JsonConvert.DeserializeObject<List<Sorter>>(rawSorters);
            }

            if (!string.IsNullOrWhiteSpace(rawEagerLoadings))
            {
                eagerLoadings = JsonConvert.DeserializeObject<List<string>>(rawEagerLoadings);
            }

            var result = await this.QueryActor.Query<PagedResult<TReadModel>>(new PagedInputForm(page, limit, filters, sorters, eagerLoadings));
            return result;
        }

        [HttpGet("{id}")]
        public virtual async Task<TReadModel> Get(Guid id, [FromQuery(Name = "eagerLoadings")] string rawEagerLoadings = null)
        {
            if (!ModelState.IsValid)
            {
                throw new CoreValidateException("参数不正确！");
            }

            var eagerLoadings = default(List<string>);

            if (!string.IsNullOrWhiteSpace(rawEagerLoadings))
            {
                eagerLoadings = JsonConvert.DeserializeObject<List<string>>(rawEagerLoadings);
            }

            var entity = await this.QueryActor.Query<TReadModel>(id);
            if (entity == null)
            {
                var result = new ActorResult();
                result.StatusCode = 404;
                result.Message = "未找找到指定数据！";
                throw result.ToException();
            }

            return entity;
        }

        [HttpPost]
        public virtual async Task Post([FromBody]JObject body)
        {
            var data = this.GetPostData(body);
            if (data.Id == default(Guid))
            {
                data.Id = SequentialGuid.Create();
            }

            var command = this.MakeCommand<CreateCommand<TAggregateRoot>>(data.Id, data);
            await command.Execute(this.CommandActor);
        }

        [HttpPut("{id}")]
        public virtual async Task Put(Guid id, [FromBody]JObject body)
        {
            if (!ModelState.IsValid)
            {
                throw new CoreValidateException("参数不正确！");
            }

            var data = this.GetPutData(body);
            var command = this.MakeCommand<UpdateCommand<TAggregateRoot>>(id, data);
            await command.Execute(this.CommandActor);
        }

        [HttpDelete("{id}")]
        public virtual async Task Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                throw new CoreValidateException("参数不正确！");
            }

            var command = this.MakeCommand<DeleteCommand<TAggregateRoot>>(id, null);
            await command.Execute(this.CommandActor);
        }

        [HttpGet("first")]
        public virtual async Task<TReadModel> FindOne([FromQuery(Name = "filters")] string rawFilters = null)
        {
            var filters = default(List<Filter>);

            if (!string.IsNullOrWhiteSpace(rawFilters))
            {
                filters = JsonConvert.DeserializeObject<List<Filter>>(rawFilters);
            }

            var result = await this.QueryActor.Query<TReadModel>(new FindSingleInputForm(filters, null));
            return result;
        }

        protected virtual TCommand MakeCommand<TCommand>(Guid aggregateId, TAggregateRoot aggregate)
            where TCommand : IAggregateRootCommand<TAggregateRoot>, new()
        {
            var command = new TCommand();

            command.AggregateRootId = aggregateId;
            command.AggregateRoot = aggregate;

            command.Timestamp = DateTime.Now;

            if (this.User.Identity.IsAuthenticated)
            {
                var claim = this.User.FindFirst(k => k.Type == ClaimTypes.PrimarySid);
                command.OperatorId = Guid.Parse(claim.Value);
                var claimName = this.User.FindFirst(k => k.Type == ClaimTypes.Name);
                command.OperatorName = claimName.Value;
            }

            long rowVersion;
            if (this.TryGetRowVersion(out rowVersion))
            {
                command.RowVersion = rowVersion;
            }

            return command;
        }

        protected virtual TCommandCommon MakeCommandCommon<TCommandCommon, TAggregateRootCommon>(Guid aggregateId, TAggregateRootCommon aggregate)
            where TAggregateRootCommon : class, IAggregateRoot, new()
            where TCommandCommon : IAggregateRootCommand<TAggregateRootCommon>, new()
        {
            var command = new TCommandCommon();

            command.AggregateRootId = aggregateId;
            command.AggregateRoot = aggregate;

            command.Timestamp = DateTime.Now;

            if (this.User.Identity.IsAuthenticated)
            {
                var claim = this.User.FindFirst(k => k.Type == ClaimTypes.PrimarySid);
                command.OperatorId = Guid.Parse(claim.Value);
                var claimName = this.User.FindFirst(k => k.Type == ClaimTypes.Name);
                command.OperatorName = claimName.Value;
            }

            long rowVersion;
            if (this.TryGetRowVersion(out rowVersion))
            {
                command.RowVersion = rowVersion;
            }

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

        protected virtual TInput GetData<TInput>(JObject body)
            where TInput : class, new()
        {
            return body.ToObject<TInput>();
        }

        protected bool TryGetRowVersion(out long rowVersion)
        {
            rowVersion = 0;
            StringValues ifMatch;
            if (this.Request.Headers.TryGetValue("If-Match", out ifMatch))
            {
                rowVersion = long.Parse(ifMatch.ToString());
                return true;
            }

            return false;
        }

        protected Guid? OperatorId
        {
            get
            {
                Guid? result = null;
                if (this.User.Identity.IsAuthenticated)
                {
                    var claim = this.User.FindFirst(k => k.Type == ClaimTypes.PrimarySid);
                    result = Guid.Parse(claim.Value);
                }
                return result;
            }
        }
    }
}
