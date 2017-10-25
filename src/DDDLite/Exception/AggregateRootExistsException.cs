namespace DDDLite.Exception
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class AggregateRootExistsException<TKey> : AggregateRootException<TKey>
  {
    public AggregateRootExistsException(TKey id) : base(id, "该数据已经存在")
    {
    }

    public AggregateRootExistsException(TKey id, string message) : base(id, message)
    {
    }
  }
}
