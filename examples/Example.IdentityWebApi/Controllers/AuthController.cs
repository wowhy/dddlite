namespace Example.IdentityWebApi.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.Exception;
    using DDDLite.WebApi.Data;
    using DDDLite.WebApi.Exception;
    using Example.IdentityWebApi.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/auth")]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly JwtBearerOptions options;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtBearerOptions options)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.options = options;
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

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new BadArgumentException("body", ModelState.First().Value.Errors.First().ErrorMessage);
            }

            var user = await userManager.FindByNameAsync(model.UserName);
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                throw new CoreException("用户名或密码错误!");
            }

            var principal = await signInManager.CreateUserPrincipalAsync(user);

            return this.Ok(new
            {
                user
            });
        }
    }
}