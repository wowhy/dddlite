namespace Sample.WebApi.Controllers
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/ping")]
    public class PingController : Controller
    {
        public PingController()
        {
        }

        [HttpGet]
        public string Get()
        {
            return "Hello, World!";
        }
    }
}