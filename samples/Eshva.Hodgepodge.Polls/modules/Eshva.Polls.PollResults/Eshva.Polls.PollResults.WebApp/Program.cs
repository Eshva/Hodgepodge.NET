#region Usings

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

#endregion


namespace Eshva.Polls.PollResults.WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            const string ApplicationEnvironmentVariablesPrefix = "POLLS_";
            return Host.CreateDefaultBuilder(args)
                       .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables(ApplicationEnvironmentVariablesPrefix))
                       .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
