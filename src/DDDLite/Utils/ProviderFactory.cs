using Autofac;

namespace DDDLite.Utils
{
    public static class ProviderFactory
    {
        private static IContainer container;

        public static TProvider Get<TProvider>()
            where TProvider : IProvider
        {
            return container.Resolve<TProvider>();
        }

        public static void SetContainer(IContainer container)
        {
            ProviderFactory.container = container;
        }
    }
}
