namespace DDDLite.Exception
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class AggregateRootNotFoundException<TKey> : AggregateRootException<TKey>
  {
    public AggregateRootNotFoundException(TKey id) : base(id, "无法找到该数据")
    {
    }

    public AggregateRootNotFoundException(TKey id, string message) : base(id, message)
    {
    }
  }
}
