namespace Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Domain.Repositories;

    public interface IAppService
    {
        IRepositoryContext Context { get; }
    }
}
