namespace DDDLite.Exception
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class AggregateRootException : CoreException
  {
    public AggregateRootException(object id, string message) : base(message)
    {
    }

    public object Id { get; protected set; }
  }

  public class AggregateRootException<TKey> : AggregateRootException
  {
    public AggregateRootException(TKey id, string message) : base(id, message)
    {
      this.Id = id;
    }

    new public TKey Id { get; protected set; }
  }
}
