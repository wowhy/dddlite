namespace TestWebApi
{
    using DDDLite.WebApi.Nancy;
    using Aggregates;
    
    
    public class UserModule : ApiModuleBase<User, User>
    {
        // 39dd052b-6670-c239-036f-2869be837e5c
        public UserModule() : base("user", TestBootstrapper.UserCommandActor, TestBootstrapper.UserQueryActor)
        {
        }
    }
}