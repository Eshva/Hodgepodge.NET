#region Usings

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eshva.Common.UnitTesting;
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
            var project = BuildDockerFile(await EmbeddedFiles.ReadAsString(ProjectTemplatePath));
            var projectFilePath = Path.Combine(CollectionFolder, ProjectFileName);
            await WriteProjectToTestCollectionFolder(project, projectFilePath);
            await UpDockerComposeFile(projectFilePath);
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
                    ConnectionMultiplexer.Connect($"{ServerHost}:{_serverPort}");
                    return Task.CompletedTask;
                }
                catch
                {
                    Console.WriteLine("Waiting for Redis 0.1s...");
                    Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                }
            }

            throw new TaskCanceledException(
                $"Unable to connect to Redis-server at {ServerHost}:{_serverPort} during {GetPreparationTimeout():g}.");
        }

        protected override TimeSpan GetPreparationTimeout() => TimeSpan.FromSeconds(13);

        protected override Task PrepareFixture()
        {
            var options = new ConfigurationOptions { AllowAdmin = true };
            options.EndPoints.Add(ServerHost, _serverPort);
            Redis = ConnectionMultiplexer.Connect(options);
            Server = Redis.GetServer(ServerHost, _serverPort);
            return Task.CompletedTask;
        }

        private string BuildDockerFile(string templateContent)
        {
            var template = Handlebars.Compile(templateContent);
            _serverPort = FreeTcpPorts.GetPorts().First();
            var data = new
                       {
                           redisPort = _serverPort
                       };

            var result = template(data);
            return result;
        }

        private Task WriteProjectToTestCollectionFolder(string projectFileContent, string projectFilePath)
        {
            using var stream = File.Open(projectFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            using var writer = new StreamWriter(stream);
            return writer.WriteAsync(projectFileContent);
        }

        private async Task UpDockerComposeFile(string dockerComposeFileName)
        {
            _fullDockerComposeFilePath = Path.Combine(CollectionFolder, dockerComposeFileName);
            var upProjectCommand = UpProjectCommand.WithFiles(_fullDockerComposeFilePath).Build();
            await upProjectCommand.Execute(TimeSpan.FromSeconds(10));
        }

        private string _fullDockerComposeFilePath;
        private int _serverPort;

        private const string ProjectTemplatePath = "polls-admin-template.yaml";
        private const string ServerHost = "localhost";
        private const string ProjectFileName = "polls-admin.yaml";
    }
}
