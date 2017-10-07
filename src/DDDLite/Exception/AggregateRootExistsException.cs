namespace DDDLite.Exception
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class AggregateRootExistsException : AggregateRootException
  {
    public AggregateRootExistsException(Guid id) : base(id, "该数据已经存在")
    {
    }

    public AggregateRootExistsException(Guid id, string message) : base(id, message)
    {
    }
  }
}
