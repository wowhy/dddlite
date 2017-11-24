using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSCore.EventSource.Commands;
using DDDLite.CQRS.Commands;
using DDDLite.CQRS.Repositories;
using DDDLite.Repositories;
using DDDLite.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Crud = CQRSCore.CRUD.Domain;
using EventSource = CQRSCore.EventSource.Domain;
namespace CQRSWebApi.Controllers
{
  [AllowAnonymous]
  public class InventoriesController : EventSourceApiController<EventSource.InventoryItem, Crud.InventoryItem>
  {
    public InventoriesController(ICommandSender commandSender, IRepository<Crud.InventoryItem, Guid> readModelRepository) : base(commandSender, readModelRepository)
    {
    }

    protected override Command GetCreateCommand(JObject model)
    {
      return new CreateInventoryItem((string)model["name"]);
    }

    protected override Command GetRemoveCommand(Guid id, long rowVersion)
    {
      throw new NotImplementedException();
    }

    protected override Command GetUpdateCommand(Guid id, long rowVersion, JsonPatchDocument patch)
    {
      throw new NotImplementedException();
    }
  }
}
