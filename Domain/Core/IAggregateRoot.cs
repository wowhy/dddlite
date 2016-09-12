namespace Domain.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAggregateRoot : IEntity
    {
        long RowVersion { get; set; }
    }
}
