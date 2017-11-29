namespace DDDLite.Querying
{
  using System.Collections.Generic;

  public class PagedResult<TData>
  {
    public int Total { get; set; }

    public List<TData> Data { get; set; }

    public PagedResult()
    { }

    public PagedResult(int total, List<TData> data)
    {
      this.Total = total;
      this.Data = data;
    }
  }
}
