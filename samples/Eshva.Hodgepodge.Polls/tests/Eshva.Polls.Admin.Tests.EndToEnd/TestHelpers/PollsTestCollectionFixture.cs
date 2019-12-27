#region Usings

using System;
using System.IO;
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
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    ConnectionMultiplexer.Connect($"{ServerHost}:{_configurationServiceDbPort}");
                    return Task.CompletedTask;
                }
                catch
                {
                    Console.WriteLine("Waiting for Redis 0.1s...");
                    Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                }
            }

            throw new TaskCanceledException(
                $"Unable to connect to Redis-server at {ServerHost}:{_configurationServiceDbPort} during {GetPreparationTimeout():g}.");
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
            _configurationServicePort = ports[1];
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

        private const string BuildServicesDockerComposeFilePathVariableName = "EshvaPollsAdminTestsEndToEnd_BuildFilePath";
        private const string PollsAdminBffImage = "polls-admin-bff:latest";
        private const string ConfigurationServiceImage = "polls-configuration-service:latest";
        private const string ProjectTemplatePath = "polls-admin-template.yaml";
        private const string ServerHost = "localhost";
        private const string ProjectFileName = "polls-admin.yaml";
    }
}
