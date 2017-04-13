namespace DDDLite.Auth
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    [ComplexType]
    public class Operator
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string LoginCode { get; set; }
    }
}
