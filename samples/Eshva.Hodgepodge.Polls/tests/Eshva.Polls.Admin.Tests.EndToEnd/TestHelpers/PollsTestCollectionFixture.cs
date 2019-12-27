#region Usings

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Eshva.Common.UnitTesting;
using Eshva.DockerCompose.Commands.BuildServices;
using Eshva.DockerCompose.Commands.DownProject;
using Eshva.DockerCompose.Commands.UpProject;
using Eshva.Hodgepodge;
using HandlebarsDotNet;
using JetBrains.Annotations;
using StackExchange.Redis;

#endregion


namespace Eshva.Polls.Admin.Tests.EndToEnd.TestHelpers
{
    [UsedImplicitly]
    public sealed class PollsTestCollectionFixture : CollectionFixtureBase
    {
        public ConnectionMultiplexer Redis { get; private set; }

        public IServer Server { get; private set; }

        protected override async Task SetupCollection()
        {
            _fullDockerComposeFilePath = Path.Combine(CollectionFolder, Path.Combine(CollectionFolder, ProjectFileName));
            await BuildServices(BuildServicesDockerComposeFilePathVariableName);
            await WriteProjectToTestCollectionFolder(
                GetRuntimeDockerComposeProject(await EmbeddedFiles.ReadAsString(ProjectTemplatePath)),
                Path.Combine(CollectionFolder, ProjectFileName));
            await UpDockerComposeFile();
        }

        protected override async Task TeardownCollection()
        {
            Redis?.Dispose();
            await DownProjectCommand.WithFiles(_fullDockerComposeFilePath).Build().Execute();
        }

        protected override Task WaitCollectionReady(CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            try
            {
                Task.WaitAll(
                    new[]
                    {
                        WaitRedisReady(),
                        WaitServiceReady(_pollsAdminBffUri, httpClient, cancellationToken),
                        WaitServiceReady(_configurationServiceUri, httpClient, cancellationToken)
                    },
                    cancellationToken);
            }
            catch (Exception exception)
            {
                throw new TaskCanceledException(
                    $"Unable to make all components ready during {GetPreparationTimeout():g}.",
                    exception);
            }

            return Task.CompletedTask;
        }

        protected override TimeSpan GetPreparationTimeout() => TimeSpan.FromSeconds(13);

        protected override Task PrepareFixture()
        {
            var options = new ConfigurationOptions { AllowAdmin = true };
            options.EndPoints.Add(ServerHost, _configurationServiceDbPort);
            Redis = ConnectionMultiplexer.Connect(options);
            Server = Redis.GetServer(ServerHost, _configurationServiceDbPort);
            return Task.CompletedTask;
        }

        private async Task WaitServiceReady(Uri serviceUri, HttpClient httpClient, CancellationToken cancellationToken)
        {
            var serviceReadinessProbeUri = new Uri(serviceUri, "readiness");
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var responseMessage = await httpClient.GetAsync(serviceReadinessProbeUri, cancellationToken);
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        return;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Error: {exception}");
                    Console.WriteLine("Waiting for Redis 100 milliseconds...");
                    await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                }
            }

            throw new TaskCanceledException(
                $"Unable to connect to service at {serviceReadinessProbeUri.AbsoluteUri} during {GetPreparationTimeout():g}.");
        }

        private Task WaitRedisReady() => ConnectionMultiplexer.ConnectAsync(CreateDbConnectionConfigurationOptions());

        private ConfigurationOptions CreateDbConnectionConfigurationOptions()
        {
            var options =
                new ConfigurationOptions
                {
                    ConnectTimeout = (int)GetPreparationTimeout().TotalMilliseconds
                };
            options.EndPoints.Add(ServerHost, _configurationServiceDbPort);
            return options;
        }

        private string GetRuntimeDockerComposeProject(string templateContent)
        {
            AssignServicePorts();

            return Handlebars.Compile(templateContent)(
                new
                {
                    pollsAdminBffImage = PollsAdminBffImage,
                    pollsAdminBffPort = _pollsAdminBffPort,
                    configurationServiceImage = ConfigurationServiceImage,
                    configurationServicePort = _configurationServicePort,
                    configurationServiceDbPort = _configurationServiceDbPort
                });
        }

        private void AssignServicePorts()
        {
            var ports = FreeTcpPorts.GetPorts(3);
            _pollsAdminBffPort = ports[0];
            _pollsAdminBffUri = new Uri($"http://localhost:{_pollsAdminBffPort}");
            _configurationServicePort = ports[1];
            _configurationServiceUri = new Uri($"http://localhost:{_configurationServicePort}");
            _configurationServiceDbPort = ports[2];
        }

        private Task WriteProjectToTestCollectionFolder(string projectFileContent, string projectFilePath)
        {
            using var stream = File.Open(projectFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            using var writer = new StreamWriter(stream);
            return writer.WriteAsync(projectFileContent);
        }

        private Task BuildServices(string buildServicesDockerComposeFilePathVariableName)
        {
            var buildServicesDockerComposeFilePath = Environment.GetEnvironmentVariable(buildServicesDockerComposeFilePathVariableName);
            return buildServicesDockerComposeFilePath == null
                ? Task.CompletedTask
                : BuildServicesCommand.WithFiles(buildServicesDockerComposeFilePath)
                                      .AllServices()
                                      .RemoveIntermediateContainers()
                                      .Build()
                                      .Execute();
        }

        private async Task UpDockerComposeFile()
        {
            var upProjectCommand = UpProjectCommand.WithFiles(_fullDockerComposeFilePath).Build();
            await upProjectCommand.Execute(TimeSpan.FromSeconds(10));
        }

        private string _fullDockerComposeFilePath;
        private int _configurationServicePort;
        private int _configurationServiceDbPort;
        private int _pollsAdminBffPort;
        private Uri _pollsAdminBffUri;
        private Uri _configurationServiceUri = new Uri("http://polls-configuration-service");

        private const string BuildServicesDockerComposeFilePathVariableName = "EshvaPollsAdminTestsEndToEnd_BuildFilePath";
        private const string PollsAdminBffImage = "polls-admin-bff:latest";
        private const string ConfigurationServiceImage = "polls-configuration-service:latest";
        private const string ProjectTemplatePath = "polls-admin-template.yaml";
        private const string ServerHost = "localhost";
        private const string ProjectFileName = "polls-admin.yaml";
    }
}
