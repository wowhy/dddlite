namespace Example.WebApi.Controllers
{
    using System;

    using DDDLite.Repositories;
    using DDDLite.WebApi.Controllers;

    using Example.Core.Domain;

    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("2.0")]
    public class ProductsController : SimpleApiController<Product>
    {
        public ProductsController(IRepository<Product> repository) : base(repository)
        {
        }
    }
}