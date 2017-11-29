using System;
using System.Globalization;

namespace DDDLite.Querying
{
  public class Sorter
  {
    public Sorter()
    {
    }

    public Sorter(string prop, SortDirection dir)
    {
      this.Property = prop;
      this.SortOrder = dir;
    }

    public string Property { get; set; }
    public SortDirection SortOrder { get; set; }
  }
}