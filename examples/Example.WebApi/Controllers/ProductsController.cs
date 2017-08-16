namespace Example.WebApi.Controllers
{
    using DDDLite.Repositories;
    using DDDLite.WebApi.Controllers;

    using Example.Core.Domain;

    public class ProductsController : SimpleApiController<Product>
    {
        public ProductsController(IRepository<Product> repository) : base(repository)
        {
        }
    }
}