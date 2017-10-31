namespace Example.WebApi.Controllers
{
    using System;
    using DDDLite.Repositories;
    using DDDLite.WebApi.Controllers;

    using Example.Core.Domain;
    using Microsoft.AspNetCore.Authorization;

    public class OrdersController : SimpleApiController<Order, Guid>
    {
        public OrdersController(IRepository<Order, Guid> repository) : base(repository)
        {
        }
    }
}