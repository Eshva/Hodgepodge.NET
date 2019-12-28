#region Usings

using Eshva.Common.WebApp;
using Eshva.Common.WebApp.ErrorHandling;
using Eshva.Common.WebApp.MediatR;
using Eshva.Common.WebApp.Readiness;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

#endregion


namespace Eshva.Polls.Admin.WebApp.Bootstrapping
{
    public static class ContainerBootstrapper
    {
        public static void ConfigureDi(this IApplicationBuilder applicationBuilder, Container container, IConfiguration configuration)
        {
            applicationBuilder.UseSimpleInjector(container);
            SetupContainer(container, configuration);
            container.Verify(VerificationOption.VerifyAndDiagnose);
        }

        private static void SetupContainer(Container container, IConfiguration configuration)
        {
            var assemblies = new[]
                             {
                                 PollsAdminWebAppAssembly.Reference,
                                 CommonWebAppAssembly.Reference
                             };
            container.AddMediatR(assemblies);
            container.AddApplicationConfiguration(configuration);
            container.AddFluentValidation(assemblies);
            container.AddDefaultReadinessTester();
        }

        private static void AddApplicationConfiguration(this Container container, IConfiguration configuration)
        {
            // TODO: Add here registration of application configuration sections.
        }
    }
}
