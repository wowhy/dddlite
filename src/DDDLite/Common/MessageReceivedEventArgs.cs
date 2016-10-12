namespace DDDLite.Common
{
    using System;

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(object message)
        {
            this.Message = message;
        }

        public object Message { get; set; }
    }
}