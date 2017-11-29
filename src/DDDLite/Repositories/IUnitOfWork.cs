namespace DDDLite.Repositories
{
  using System.Threading;
  using System.Threading.Tasks;

  public interface IUnitOfWork
  {
    Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
  }
}