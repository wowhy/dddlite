namespace DDDLite
{
    using System;

    public abstract class DisposableObject : IDisposable
    {
        ~DisposableObject()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }
    }
}