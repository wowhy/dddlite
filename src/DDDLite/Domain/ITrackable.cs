namespace DDDLite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITrackable
    {
        DateTime CreatedAt { get; set; }
        string CreatedById { get; set; }

        DateTime LastUpdatedAt { get; set; }
        string LastUpdatedById { get; set; }
    }
}
