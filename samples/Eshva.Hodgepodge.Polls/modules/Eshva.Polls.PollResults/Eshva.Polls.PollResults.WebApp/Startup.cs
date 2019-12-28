#region Usings

using Eshva.Common.WebApp;
using Eshva.Common.WebApp.ErrorHandling;
using Eshva.Polls.PollResults.WebApp.Bootstrapping;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;

#endregion


namespace Eshva.Polls.PollResults.WebApp
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(CommonWebAppAssembly.Reference); // TODO: Try to use AddMvcCore only.
            services.AddSimpleInjector(
                _container,
                options =>
                {
                    options.AddAspNetCore()
                           .AddControllerActivation();
                    options.AddLogging();
                });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {
            applicationBuilder.ConfigureDi(_container, Configuration);
            applicationBuilder.UseMiddleware<FluentValidationExceptionHandlerMiddleware>();
            applicationBuilder.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private readonly Container _container = new Container();
    }
}
