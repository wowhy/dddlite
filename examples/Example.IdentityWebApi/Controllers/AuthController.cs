namespace Example.IdentityWebApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DDDLite.WebApi.Data;
    using DDDLite.WebApi.Exception;
    using Example.IdentityWebApi.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/auth")]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
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

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) 
            {
                throw new BadArgumentException("body", result.Errors.First().Description);
            }

            return this.Ok();
        }
    }
}