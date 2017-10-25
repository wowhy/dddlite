namespace DDDLite.WebApi.Provider
{
    using System;

    public interface ICurrentUserProvider
    {
        string GetCurrentUserId();
    }
}