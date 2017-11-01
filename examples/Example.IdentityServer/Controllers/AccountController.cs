namespace Example.IdentityServer.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AspNet.Security.OAuth.Validation;
    using Example.IdentityServer.Data;
    using DDDLite.WebApi.Exception;
    using Example.IdentityServer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/account")]
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new BadArgumentException("body", ModelState.First().Value.Errors.First().ErrorMessage);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new BadArgumentException("body", result.Errors.First().Description);
            }

            return this.Ok();
        }
    }
}