namespace DDDLite.CQRS.Commands
{
  public class Command : ICommand
  {
    public long RowVersion { get; set; }
  }
}