namespace DDDLite.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Extensions.PlatformAbstractions;

    using DDDLite.Domain;
    using DDDLite.Exception;
    using DDDLite.Repositories;
    using DDDLite.Specifications;
    using DDDLite.WebApi.Internal;
    using DDDLite.WebApi.Internal.Query;
    using DDDLite.WebApi.Exception;
    using DDDLite.WebApi.Provider;
    using DDDLite.WebApi.Models;

    using @N = DDDLite.WebApi.Internal.ApiParams;

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class SimpleApiController<TAggregateRoot, TKey> : Controller
            where TAggregateRoot : class, IAggregateRoot<TKey>
            where TKey : IEquatable<TKey>
    {
        private readonly IRepository<TAggregateRoot, TKey> repository;

        protected IRepository<TAggregateRoot, TKey> Repository => this.repository;

        public SimpleApiController(IRepository<TAggregateRoot, TKey> repository)
        {
            this.repository = repository;

        }

        [HttpGet]
        public virtual IActionResult Get()
        {
            var context = new RepositoryQueryContext<TAggregateRoot, TKey>(Repository, HttpContext);
            var values = context.GetValues();

            return Ok(values);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(TKey id)
        {
            var context = new RepositoryQueryContext<TAggregateRoot, TKey>(Repository, HttpContext);
            var value = await context.GetValueAsync(id);

            this.Response.Headers.Add("ETag", new StringValues(value.Value.RowVersion.ToString()));

            return Ok(value);
        }

        [HttpPost]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Post([FromBody] TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot is IAggregateRoot<Guid> && ((IAggregateRoot<Guid>)aggregateRoot).Id == Guid.Empty)
            {
                ((IAggregateRoot<Guid>)aggregateRoot).NewIdentity();
            }

            if (aggregateRoot is IAggregateRoot<string> && string.IsNullOrEmpty(((IAggregateRoot<string>)aggregateRoot).Id))
            {
                ((IAggregateRoot<string>)aggregateRoot).NewIdentity();
            }

            if (Repository.Exists(Specification<TAggregateRoot>.Eval(k => k.Id.Equals(aggregateRoot.Id))))
            {
                throw new AggregateRootExistsException<TKey>(aggregateRoot.Id);
            }

            aggregateRoot.CreatedAt = DateTime.Now;
            aggregateRoot.CreatedById = GetCurrentUserId();
            aggregateRoot.LastUpdatedAt = aggregateRoot.CreatedAt;
            aggregateRoot.LastUpdatedById = aggregateRoot.LastUpdatedById;

            await AddAsync(aggregateRoot);

            return Created(Url.Action("Get", new { id = aggregateRoot.Id }), new ResponseValue<TKey>(aggregateRoot.Id));
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Put(TKey id, [FromHeader(Name = @N.ROWVERSION)] string concurrencyToken, [FromBody] TAggregateRoot aggregateRoot)
        {
            if (!Repository.Exists(Specification<TAggregateRoot>.Eval(k => k.Id.Equals(id))))
            {
                throw new AggregateRootNotFoundException<TKey>(id);
            }

            if (concurrencyToken == null)
            {
                throw new BadArgumentException(@N.ROWVERSION);
            }

            aggregateRoot.Id = id;
            aggregateRoot.LastUpdatedAt = DateTime.Now;
            aggregateRoot.LastUpdatedById = GetCurrentUserId();
            aggregateRoot.RowVersion = long.Parse(concurrencyToken);

            await UpdateAsync(aggregateRoot);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Patch(TKey id, [FromHeader(Name = @N.ROWVERSION)] string concurrencyToken, [FromBody] JsonPatchDocument<TAggregateRoot> patch)
        {
            var aggregateRoot = await Repository.GetByIdAsync(id);
            if (aggregateRoot == null)
            {
                throw new AggregateRootNotFoundException<TKey>(id);
            }

            if (concurrencyToken == null)
            {
                throw new BadArgumentException(@N.ROWVERSION);
            }

            patch.ApplyTo(aggregateRoot);

            aggregateRoot.Id = id;
            aggregateRoot.LastUpdatedAt = DateTime.Now;
            aggregateRoot.LastUpdatedById = GetCurrentUserId();
            aggregateRoot.RowVersion = long.Parse(concurrencyToken);

            await UpdateAsync(aggregateRoot);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TKey id, [FromHeader(Name = @N.ROWVERSION)] string concurrencyToken)
        {
            var aggregateRoot = await Repository.GetByIdAsync(id);
            if (aggregateRoot == null)
            {
                throw new AggregateRootNotFoundException<TKey>(id);
            }

            if (concurrencyToken == null)
            {
                throw new BadArgumentException(@N.ROWVERSION);
            }

            aggregateRoot.LastUpdatedAt = DateTime.Now;
            aggregateRoot.LastUpdatedById = GetCurrentUserId();
            aggregateRoot.RowVersion = long.Parse(concurrencyToken);

            await DeleteAsync(aggregateRoot);

            return NoContent();
        }

        protected virtual string GetCurrentUserId()
        {
            var provider = HttpContext.RequestServices.GetService<ICurrentUserProvider>();
            return provider.GetCurrentUserId();
        }

        protected virtual async Task AddAsync(TAggregateRoot aggregateRoot) 
        {
            await Repository.AddAsync(aggregateRoot);
            await Repository.UnitOfWork.CommitAsync();
        }

        protected virtual async Task UpdateAsync(TAggregateRoot aggregateRoot) 
        {
            await Repository.UpdateAsync(aggregateRoot);
            await Repository.UnitOfWork.CommitAsync();
        }

        protected virtual async Task DeleteAsync(TAggregateRoot aggregateRoot) 
        {
            await Repository.DeleteAsync(aggregateRoot);
            await Repository.UnitOfWork.CommitAsync();
        }
    }
}