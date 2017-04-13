using DDDLite.WebApi.Mvc;
using Example.Core.Actors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Core.Commands;
using Example.WebApi.Models;
using DDDLite.Validation;
using DDDLite;
using System.ComponentModel.DataAnnotations;
using Akka.Actor;
using DDDLite.Actors;

namespace Example.WebApi.Controllers
{
    [Route("/api/users")]
    public class UserController : ApiControllerBase<Core.Domain.User, Core.Querying.User>
    {
        public UserController() : base(
            Startup.System.GetActor(ActorPaths.UserCommand),
            Startup.System.GetActor(ActorPaths.UserQuery))
        {
        }

        [HttpPut("{id}/changepwd")]
        public async Task<ActorResult> ChangePassword(Guid id, [FromBody]ChangePasswordInput input)
        {
            var errors = new List<ValidationResult>();
            if (!input.TryValidate(errors))
            {
                throw new CoreValidateException(errors.First().ErrorMessage);
            }

            //var user = this.QueryActor.Ask();

            var command = new ChangePasswordCommand()
            {
                AggregateRootId = id,
                NewPassword = input.NewPassword,
                OldPassword = input.OldPassword
            };

            var result = await this.CommandActor.Ask<ActorResult>(command);
            if (!result.Successed)
            {
                throw result.ToException();
            }

            return result;
        }
    }
}
