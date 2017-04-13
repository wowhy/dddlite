using DDDLite.Actors;
using DDDLite.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDLite.Commands;
using DDDLite.Specifications;

namespace Example.Actors
{
    public class UserValidateActor : ValidateActor
    {
        private IQueryService<Core.Querying.User> queryService;
        public UserValidateActor(IQueryService<Core.Querying.User> queryService)
        {
            this.queryService = queryService;
        }

        protected override void InitReceiveMessages()
        {
            ValidateAsync<CreateCommand<Core.Domain.User>>(async (command) =>
            {
                if (string.IsNullOrEmpty(command.AggregateRoot.Code))
                {
                    return ActorResult.Failure("用户名和密码不能为空");
                }

                var count = await this.queryService.CountAsync(Specification<Core.Querying.User>.Eval(k => k.Code == command.AggregateRoot.Code));
                if (count != 0)
                {
                    return ActorResult.Failure("用户名已经存在，请重新输入！");
                }

                return ActorResult.Success();
            });
        }


    }
}
