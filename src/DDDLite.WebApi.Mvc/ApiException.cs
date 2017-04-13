using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDLite.WebApi
{
    public class ApiException : CoreException
    {
        public int StatusCode { get; protected set; }
        public object ErrorModel { get; protected set; }

        public ApiException(int statusCode, object errorModel)
        {
            this.StatusCode = statusCode;
            this.ErrorModel = errorModel;
        }
    }
}
