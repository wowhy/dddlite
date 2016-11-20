namespace DDDLite.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    using Commands;

    public sealed class CommandConsumer : DisposableObject, ICommandConsumer
    {
        private bool disposed;

        public IMessageSubscriber Subscriber { get; private set; }

        private ICommandHandlerFactory factory;

        public CommandConsumer(IMessageSubscriber subscriber, ICommandHandlerFactory factory)
        {
            this.Subscriber = subscriber;
            this.factory = factory;

            subscriber.MessageReceived += (sender, e) =>
            {
                var messageType = e.Message.GetType();
                var handler = factory.GetHandler(messageType);
                if (handler != null)
                {
                    var method = handler.GetType().GetRuntimeMethod("Handle", new Type[] { messageType });
                    if (method != null)
                    {
                        try
                        {
                            method.Invoke(handler, new[] { e.Message });
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                    }
                    else
                    {
                        throw new CoreException("命令处理程序缺少Handle方法！");
                    }
                }
                else
                {
                    throw new CoreException("无法初始化命令处理程序！");
                }
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    this.Subscriber.Dispose();
                    disposed = true;
                }
            }
        }
    }
}
