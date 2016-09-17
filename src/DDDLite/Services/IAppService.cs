namespace DDDLite.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Repositories;

    public interface IAppService
    {
        IRepositoryContext Context { get; }
    }
}
