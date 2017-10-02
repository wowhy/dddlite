namespace Example.WebApi.Controllers
{
    using System;
    using DDDLite.Repositories;
    using DDDLite.WebApi.Controllers;

    using Example.Core.Domain;
    using Microsoft.AspNetCore.Authorization;

    public class ProductsController : SimpleApiController<Product>
    {
        public ProductsController(IRepository<Product> repository) : base(repository)
        {
        }
    }
}