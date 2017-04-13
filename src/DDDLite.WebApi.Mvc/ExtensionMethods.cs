namespace DDDLite.WebApi.Mvc
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Actors;
    using Akka.Actor;
    using Commands;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;

    public static class ExtensionMethods
    {
        public static async Task<TData> Query<TData>(this ICanTell @this, object form)
        {
            var result = await @this.Ask<ActorResult<TData>>(form);
            if (!result.Successed)
            {
                throw ToException(result);
            }

            return result.Result;
        }

        public static async Task Execute(this ICommand @this, ICanTell actor)
        {
            var result = await actor.Ask<ActorResult>(@this);
            if (!result.Successed)
            {
                throw ToException(result);
            }
        }

        public static async Task<TData> Execute<TData>(this ICommand @this, ICanTell actor)
        {
            var result = await actor.Ask<ActorResult>(@this);
            if (!result.Successed)
            {
                throw ToException(result);
            }

            return (result as ActorResult<TData>).Result;
        }

        public static Exception ToException(this ActorResult result)
        {
            return new ApiException(result.StatusCode, result);
        }

        public static DDDLite.Auth.RBACUser GetUserData(this ClaimsPrincipal @this)
        {
            var claim = @this.FindFirst(k => k.Type == ClaimTypes.UserData);
            if (claim != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<DDDLite.Auth.RBACUser>(claim.Value);
            }

            return null;
        }
    }
}
