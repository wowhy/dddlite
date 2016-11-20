namespace DDDLite.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthorizedException : CoreException
    {
        private int status;

        public AuthorizedException(int status, string message)
            : base(message)
        {
            this.status = status;
        }

        public AuthorizedException(int status, string message, Exception innerException)
            : base(message, innerException)
        {
            this.status = status;
        }

        public int Status => this.status;
    }
}
