namespace EventManagement.TestAutomation.TestCore.Base
{
    using Microsoft.Practices.Unity;

    public static class UnityContainerFactory
    {
        private static IUnityContainer unityContainer;

        static UnityContainerFactory()
        {
            unityContainer = new UnityContainer();
        }

        public static IUnityContainer GetContainer()
        {
            return unityContainer;
        }
    }
}
