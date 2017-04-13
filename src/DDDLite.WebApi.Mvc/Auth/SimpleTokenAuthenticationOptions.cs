namespace DDDLite.WebApi.Mvc.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;

    public class SimpleTokenAuthenticationOptions : AuthenticationOptions
    {
        public SimpleTokenAuthenticationOptions()
        {
            this.AutomaticAuthenticate = true;
            this.AutomaticChallenge = true;
        }

        public Func<ITokenValidator> TokenValidatorFactory { get; set; }
    }
}
