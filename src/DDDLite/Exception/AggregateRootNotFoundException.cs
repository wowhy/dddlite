namespace DDDLite.Exception
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class AggregateRootNotFoundException : AggregateRootException
  {
    public AggregateRootNotFoundException(Guid id) : base(id, "无法找到该数据")
    {

    }

    public AggregateRootNotFoundException(Guid id, string message) : base(id, message)
    {
    }
  }
}
