namespace DDDLite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITrackable
    {
        DateTime CreatedAt { get; set; }
        Guid? CreatedById { get; set; }

        DateTime LastUpdatedAt { get; set; }
        Guid? LastUpdatedById { get; set; }
    }
}
