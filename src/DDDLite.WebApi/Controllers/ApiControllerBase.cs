namespace DDDLite.WebApi.Controllers
{
  using System;

  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.Extensions.DependencyInjection;

  using DDDLite.WebApi.Provider;

  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  [Authorize]
  public abstract class ApiControllerBase : Controller
  {
    protected virtual string GetAuthUserId()
    {
      var provider = HttpContext.RequestServices.GetService<IOperatorProvider>();
      return provider.GetCurrentOperator()?.UserId;
    }
  }
}