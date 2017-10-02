namespace Example.WebApi.Controllers
{
    using System;
    using DDDLite.Repositories;
    using DDDLite.WebApi.Controllers;

    using Example.Core.Domain;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class OrdersController : SimpleApiController<Order>
    {
        public OrdersController(IRepository<Order> repository) : base(repository)
        {
        }
    }
}