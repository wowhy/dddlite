namespace DDDLite.WebApi.Controllers
{
    using System;    
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.Extensions.Primitives;

    using DDDLite.Domain;
    using DDDLite.Exception;
    using DDDLite.Repositories;
    using DDDLite.Specifications;
    using DDDLite.WebApi.Internal;   

    using  @N = DDDLite.WebApi.Internal.ApiParams;
    using DDDLite.WebApi.Internal.Query;

    [Route("api/[controller]")]
    public class SimpleApiController<TAggregateRoot> : Controller
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IRepository<TAggregateRoot> repository;

        protected IRepository<TAggregateRoot> Repository => this.repository;

        public SimpleApiController(IRepository<TAggregateRoot> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public virtual IActionResult Get()
        {
            var context = new RepositoryQueryContext<TAggregateRoot>(Repository, HttpContext);
            var values = context.GetValues();

            return Ok(values);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            var context = new RepositoryQueryContext<TAggregateRoot>(Repository, HttpContext);
            var value = await context.GetValueAsync(id);

            return Ok(value);
        }

        [HttpPost("{id}")]
        public virtual async Task<IActionResult> Post([FromBody] TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot.Id == Guid.Empty)
            {
                aggregateRoot.NewIdentity();
            }

            if (Repository.Exists(Specification<TAggregateRoot>.Eval(k => k.Id == aggregateRoot.Id)))
            {
                throw new AggregateExistsException(aggregateRoot.Id);
            }

            aggregateRoot.CreatedAt = DateTime.Now;
            aggregateRoot.CreatedById = GetCurrentUserId();
            aggregateRoot.LastUpdatedAt = aggregateRoot.CreatedAt;
            aggregateRoot.LastUpdatedById = aggregateRoot.LastUpdatedById;

            await Repository.AddAsync(aggregateRoot);

            return Created(Url.Action("Get", new { id = aggregateRoot.Id }), aggregateRoot.Id);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(Guid id, [FromHeader(Name = @N.ROWVERSION)] long concurrencyToken, [FromBody] TAggregateRoot aggregateRoot)
        {
            if (Repository.Exists(Specification<TAggregateRoot>.Eval(k => k.Id == id)))
            {
                throw new AggregateNotFoundException(id);
            }

            aggregateRoot.Id = id;
            aggregateRoot.LastUpdatedAt = DateTime.Now;
            aggregateRoot.LastUpdatedById = this.GetCurrentUserId();
            aggregateRoot.RowVersion = concurrencyToken;

            await Repository.UpdateAsync(aggregateRoot);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch(Guid id, [FromHeader(Name = @N.ROWVERSION)] long concurrencyToken, [FromBody] JsonPatchDocument<TAggregateRoot> patch)
        {
            var aggregateRoot = await Repository.GetByIdAsync(id);
            if (aggregateRoot == null)
            {
                throw new AggregateNotFoundException(id);
            }

            patch.ApplyTo(aggregateRoot);
            
            aggregateRoot.Id = id;
            aggregateRoot.LastUpdatedAt = DateTime.Now;
            aggregateRoot.LastUpdatedById = this.GetCurrentUserId();
            aggregateRoot.RowVersion = concurrencyToken;

            await Repository.UpdateAsync(aggregateRoot);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id, [FromHeader(Name = @N.ROWVERSION)] long concurrencyToken)
        {
            var aggregateRoot = await Repository.GetByIdAsync(id);
            if (aggregateRoot == null)
            {
                throw new AggregateNotFoundException(id);
            }

            aggregateRoot.LastUpdatedAt = DateTime.Now;
            aggregateRoot.LastUpdatedById = this.GetCurrentUserId();
            aggregateRoot.RowVersion = concurrencyToken;

            await Repository.DeleteAsync(aggregateRoot);

            return NoContent();
        }

        protected virtual Guid? GetCurrentUserId()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var claim = this.User.FindFirst(k => k.Type == ClaimTypes.NameIdentifier);
                return Guid.Parse(claim.Value);
            }

            return null;
        }
    }
}