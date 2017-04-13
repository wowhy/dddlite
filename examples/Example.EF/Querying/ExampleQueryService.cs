namespace Example.EF.Querying
{
    using DDDLite.Querying;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.Specifications;
    using DDDLite.Querying.EF;
    using Core.Querying;

    using Microsoft.EntityFrameworkCore;

    public class ExampleQueryService<TReadModel> : EFQueryService<TReadModel>
        where TReadModel : class, new()
    {
        public ExampleQueryService(ReadDbContext context) : base(context)
        {
        }
    }
}
