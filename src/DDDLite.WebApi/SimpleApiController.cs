namespace DDDLite.WebApi
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using DDDLite.Domain;
    using DDDLite.Repositories;
    using DDDLite.Specifications;
    using System.Security.Claims;

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
        public virtual IActionResult Get(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string query = "",
            [FromQuery] string sort = "")
        {
            var result = Repository.PagedSearch(page, limit, Specification<TAggregateRoot>.Any(), SortSpecification<TAggregateRoot>.None);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            var aggregateRoot = await Repository.GetByIdAsync(id);
            return Ok(aggregateRoot);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Post([FromBody] TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot.Id == Guid.Empty)
            {
                aggregateRoot.NewIdentity();
            }

            if (Repository.Exists(Specification<TAggregateRoot>.Eval(k => k.Id == aggregateRoot.Id)))
            {
                throw new ArgumentException(nameof(aggregateRoot.Id));
            }

            aggregateRoot.CreatedAt = DateTime.Now;
            aggregateRoot.CreatedById = GetCurrentUserId();

            await Repository.AddAsync(aggregateRoot);
            return Created(Url.Action("Get", new { id = aggregateRoot.Id }), aggregateRoot.Id);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(Guid id, [FromBody] TAggregateRoot aggregateRoot)
        {
            aggregateRoot.Id = id;
            aggregateRoot.LastUpdatedAt = DateTime.Now;
            aggregateRoot.LastUpdatedById = this.GetCurrentUserId();

            await Repository.UpdateAsync(aggregateRoot);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public virtual Task<IActionResult> Patch(Guid id)
        {
            return null;
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            var aggregateRoot = await Repository.GetByIdAsync(id);
            if (aggregateRoot == null)
            {
                throw new ArgumentException(nameof(id));
            }

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