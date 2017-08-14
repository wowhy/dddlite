namespace DDDLite.WebApi.Config
{
    public class ApiConfig
    {
        private static readonly ApiConfig @default = new ApiConfig();

        public static ApiConfig Default => @default;

        internal ApiConfig()
        {
        }

        public int MaxQueryCount { get; set; } = 10000;

        public int DefaultPageSize { get; set; } = 10;
    }
}