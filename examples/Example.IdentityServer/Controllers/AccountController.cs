namespace Example.IdentityServer.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.WebApi.Data;
    using DDDLite.WebApi.Exception;
    using Example.IdentityServer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("~/connect/register")]
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