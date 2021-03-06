namespace Example.IdentityServer.Controllers
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;
  using AspNet.Security.OAuth.Validation;
  using AspNet.Security.OpenIdConnect.Primitives;
  using Example.IdentityServer.Data;
  using DDDLite.WebApi.Models;
  using Microsoft.AspNetCore.Authentication.JwtBearer;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Newtonsoft.Json.Linq;
  using OpenIddict.Core;

  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/userinfo")]
  [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
  public class UserinfoController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public UserinfoController(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    //
    // GET: /api/userinfo
    [HttpGet, Produces("application/json")]
    public async Task<IActionResult> Userinfo()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return BadRequest(new ResponseError
        {
          Error = new ErrorData
          {
            Code = OpenIdConnectConstants.Errors.InvalidGrant,
            Message = "用户个人信息已经无效。"
          }
        });
      }

      var claims = new JObject();

      // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
      claims[OpenIdConnectConstants.Claims.Subject] = await _userManager.GetUserIdAsync(user);
      claims[OpenIdConnectConstants.Claims.Name] = await _userManager.GetUserNameAsync(user);

      if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Email))
      {
        claims[OpenIdConnectConstants.Claims.Email] = await _userManager.GetEmailAsync(user);
        claims[OpenIdConnectConstants.Claims.EmailVerified] = await _userManager.IsEmailConfirmedAsync(user);
      }

      if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Phone))
      {
        claims[OpenIdConnectConstants.Claims.PhoneNumber] = await _userManager.GetPhoneNumberAsync(user);
        claims[OpenIdConnectConstants.Claims.PhoneNumberVerified] = await _userManager.IsPhoneNumberConfirmedAsync(user);
      }

      if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIddictConstants.Scopes.Roles))
      {
        claims["roles"] = JArray.FromObject(await _userManager.GetRolesAsync(user));
      }

      // Note: the complete list of standard claims supported by the OpenID Connect specification
      // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

      return Json(new ResponseValue<JObject>(claims));
    }
  }
}