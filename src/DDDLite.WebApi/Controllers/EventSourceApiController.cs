namespace DDDLite.WebApi.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Security.Claims;
  using System.Threading.Tasks;

  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.JsonPatch;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Primitives;
  using Microsoft.Extensions.PlatformAbstractions;

  using Newtonsoft.Json.Linq;

  using DDDLite.Domain;
  using DDDLite.CQRS;
  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Repositories;
  using DDDLite.Repositories;
  using DDDLite.WebApi.Internal.Query;
  using DDDLite.WebApi.Models;

  using @N = DDDLite.WebApi.Internal.ApiParams;

  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  [Authorize]
  public abstract class EventSourceApiController<TEventSource, TReadModel> : ApiControllerBase
    where TEventSource : class, IEventSource, new()
    where TReadModel : class, IAggregateRoot<Guid>
  {
    private readonly ICommandSender commandSender;
    private readonly IRepository<TReadModel, Guid> readModelRepository;

    public EventSourceApiController(
      ICommandSender commandSender,
      IRepository<TReadModel, Guid> readModelRepository
    )
    {
      this.commandSender = commandSender;
      this.readModelRepository = readModelRepository;
    }

    protected ICommandSender CommandSender => this.CommandSender;

    protected IRepository<TReadModel, Guid> ReadModelRepository => this.readModelRepository;

    [HttpGet]
    public virtual IActionResult Get()
    {
      var context = new RepositoryQueryContext<TReadModel, Guid>(readModelRepository, HttpContext);
      var values = context.GetValues();

      return Ok(values);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
      var context = new RepositoryQueryContext<TReadModel, Guid>(readModelRepository, HttpContext);
      var value = await context.GetValueAsync(id);

      this.Response.Headers.Add("ETag", new StringValues(value.Value.RowVersion.ToString()));

      return Ok(value);
    }

    [HttpPost]
    [Produces("application/json")]
    public virtual async Task<IActionResult> Post([FromBody]JObject model)
    {
      var command = this.GetCreateCommand(model);
      command.OperatorId = this.GetAuthUserId();
      if (command.Id == Guid.Empty)
      {
        command.Id = SequentialGuid.Create();
      }

      await this.commandSender.SendAsync(command);

      return Created(Url.Action("Get", new { id = command.Id }), new ResponseValue<Guid>(command.Id));
    }

    [HttpPatch("{id}")]
    [Produces("application/json")]
    public virtual async Task<IActionResult> Patch(Guid id, [FromHeader(Name = @N.ROWVERSION)] string concurrencyToken, [FromBody] JsonPatchDocument patch)
    {
      var rowVersion = long.Parse(concurrencyToken);
      var command = this.GetUpdateCommand(id, rowVersion, patch);
      command.Id = id;
      command.OriginalVersion = rowVersion;
      command.OperatorId = this.GetAuthUserId();

      await this.commandSender.SendAsync(command);

      return NoContent();
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id, [FromHeader(Name = @N.ROWVERSION)] string concurrencyToken)
    {
      var rowVersion = long.Parse(concurrencyToken);
      var command = this.GetDeleteCommand(id, rowVersion);
      command.Id = id;
      command.OriginalVersion = rowVersion;
      command.OperatorId = this.GetAuthUserId();

      await this.commandSender.SendAsync(command);

      return NoContent();
    }

    protected abstract Command GetCreateCommand(JObject model);

    protected abstract Command GetUpdateCommand(Guid id, long rowVersion, JsonPatchDocument patch);

    protected abstract Command GetDeleteCommand(Guid id, long rowVersion);
  }
}