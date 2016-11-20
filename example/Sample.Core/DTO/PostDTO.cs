namespace Sample.Core.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite;

    public class PostDTO : DTOBase
    {
        public string Title { get; set; }

        public string Blog_Title { get; set; }
    }
}
