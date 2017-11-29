namespace DDDLite.CQRS.Events
{
  using System;
  using System.Threading.Tasks;

  using DDDLite.CQRS.Events;
  using DDDLite.Domain;
  using DDDLite.Repositories;

  public abstract class DtoEventHandlers<TReadModel>
    where TReadModel : class, IDto, new()
  {
    private readonly IRepository<TReadModel, Guid> repository;

    public IRepository<TReadModel, Guid> Repository => this.repository;

    protected DtoEventHandlers(IRepository<TReadModel, Guid> repository)
    {
      this.repository = repository;
    }

    protected Task<TReadModel> GetAsync(IEvent @event, params string[] includes)
    {
      return this.Repository.GetByIdAsync(@event.Id, includes);
    }

    protected async Task CreateAsync(IEvent @event, TReadModel dto)
    {
      dto.CreatedAt = dto.LastUpdatedAt = @event.Timestamp;
      dto.CreatedById = dto.LastUpdatedById = @event.OperatorId;

      await this.repository.InsertAsync(dto);
      await this.repository.UnitOfWork.CommitAsync();
    }

    protected async Task UpdateAsync(IEvent @event, TReadModel dto)
    {
      dto.LastUpdatedAt = @event.Timestamp;
      dto.LastUpdatedById = @event.OperatorId;
      dto.Version = @event.Version;

      await this.repository.UpdateAsync(dto);
      await this.repository.UnitOfWork.CommitAsync();
    }

    protected async Task RemoveAsync(IEvent @event, TReadModel dto)
    {
      dto.LastUpdatedAt = @event.Timestamp;
      dto.LastUpdatedById = @event.OperatorId;
      dto.Version = @event.Version;

      await this.repository.DeleteAsync(dto);
      await this.repository.UnitOfWork.CommitAsync();
    }
  }
}