namespace Example.WebApi.Controllers
{
    using System;
    using DDDLite.Repositories;
    using DDDLite.WebApi.Controllers;

    using Example.Core.Domain;
    using Microsoft.AspNetCore.Authorization;

    public class ProductsController : SimpleApiController<Product, Guid>
    {
        public ProductsController(IRepository<Product, Guid> repository) : base(repository)
        {
        }
    }
}