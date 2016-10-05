namespace Sample.WebApi.Controllers
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/ping")]
    public class PingController : Controller
    {
        private readonly IHostingEnvironment env;

        public PingController(IHostingEnvironment env)
        {
            this.env = env;
        }

        [HttpGet]
        public string Get()
        {
            Console.WriteLine(env.ApplicationName);
            return env.ApplicationName;
        }
    }
}