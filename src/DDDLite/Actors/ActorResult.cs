namespace DDDLite.Actors
{
    using Querying;

    public class ActorResult
    {
        public int StatusCode { get; set; } = 500;

        public bool Successed { get; set; }

        public string Message { get; set; }

        public string[] Details { get; set; }

        public string DebugMessage { get; set; }

        public string StackTrace { get; set; }

        public static ActorResult Success(string message = null)
        {
            return new ActorResult()
            {
                Successed = true,
                StatusCode = 200,
                Message = message
            };
        }

        public static ActorResult Failure(string message = null)
        {
            return new ActorResult()
            {
                Successed = false,
                Message = message
            };
        }
    }

    public class ActorResult<TResult> : ActorResult
    {
        public ActorResult()
        {
        }

        public TResult Result { get; set; }

        public static ActorResult<TResult> Success(TResult result = default(TResult), string message = null)
        {
            return new ActorResult<TResult>()
            {
                Successed = true,
                StatusCode = 200,
                Message = message,
                Result = result
            };
        }

        new public static ActorResult<TResult> Failure(string message = null)
        {
            return new ActorResult<TResult>()
            {
                Successed = false,
                Message = message
            };
        }
    }
}