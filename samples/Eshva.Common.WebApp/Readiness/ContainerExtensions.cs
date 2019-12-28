#region Usings

using SimpleInjector;

#endregion


namespace Eshva.Common.WebApp.Readiness
{
    public static class ContainerExtensions
    {
        public static void AddDefaultReadinessTester(this Container container)
        {
            container.RegisterSingleton<IReadinessTester, DefaultReadinessTester>();
        }
    }
}
