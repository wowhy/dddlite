namespace DDDLite
{
    using System;
    using Newtonsoft.Json;

    public class DTOBase : IConcurrencyVersion
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public long RowVersion { get; set; }
    }
}
