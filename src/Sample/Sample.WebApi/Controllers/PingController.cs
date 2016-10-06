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
            return this.env.EnvironmentName;
        }

        [HttpGet]
        [Route("exception")]
        public void ThrowException()
        {
            throw new Exception("测试异常信息！");
        }
    }
}