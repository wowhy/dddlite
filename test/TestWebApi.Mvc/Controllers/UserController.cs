using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using DDDLite.WebApi.Mvc;
using TestCommand.Aggregates;
using DDDLite.Actors;
using Microsoft.AspNetCore.Mvc;

namespace TestWebApi.Mvc.Controllers
{
    [Route("/api/users")]
    public class UserController : ApiControllerBase<User, User>
    {
        public UserController() : base(null, null)
        {
        }
    }
}
