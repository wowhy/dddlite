namespace DDDLite.WebApi.Data
{

    using System;
    using DDDLite.Domain;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole<Guid>, IEntity
    {
    }
}