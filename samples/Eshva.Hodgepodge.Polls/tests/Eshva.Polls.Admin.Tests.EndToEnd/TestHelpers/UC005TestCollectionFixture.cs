#region Usings

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Eshva.Common.UnitTesting;
using Eshva.DockerCompose.Commands.DownProject;
using Eshva.DockerCompose.Commands.UpProject;
using Eshva.Hodgepodge;
using JetBrains.Annotations;
using StackExchange.Redis;
using Xunit;

#endregion


namespace Eshva.Polls.Admin.Tests.EndToEnd.TestHelpers
{
    [UsedImplicitly]
    [Collection(nameof(UC005TestCollection))]
    public sealed class UC005TestCollectionFixture : CollectionFixtureBase
    {
        public ConnectionMultiplexer Redis { get; private set; }

        public IServer Server { get; private set; }

        protected override Task SetupCollection()
        {
            EmbeddedFiles.SaveToFolder(CollectionFolder, "TestHelpers/polls-admin.yaml");
            return UpDockerComposeFile("polls-admin.yaml");
        }

        protected override Task TeardownCollection()
        {
            Redis?.Dispose();
            return DownProjectCommand.WithFiles(_fullDockerComposeFilePath).Build().Execute();
        }

        protected override Task WaitCollectionReady(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    ConnectionMultiplexer.Connect($"{ServerHost}:{ServerPort}");
                    return Task.CompletedTask;
                }
                catch
                {
                    Console.WriteLine("Waiting for Redis 0.1s...");
                    Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                }
            }

            throw new TaskCanceledException(
                $"Unable to connect to Redis-server at {ServerHost}:{ServerPort} during {GetPreparationTimeout():g}.");
        }

        protected override TimeSpan GetPreparationTimeout() => TimeSpan.FromSeconds(13);

        protected override Task PrepareFixture()
        {
            var options = new ConfigurationOptions { AllowAdmin = true };
            options.EndPoints.Add(ServerHost, ServerPort);
            Redis = ConnectionMultiplexer.Connect(options);
            Server = Redis.GetServer(ServerHost, ServerPort);
            return Task.CompletedTask;
        }

        private async Task UpDockerComposeFile(string dockerComposeFileName)
        {
            _fullDockerComposeFilePath = Path.Combine(CollectionFolder, dockerComposeFileName);
            var upProjectCommand = UpProjectCommand.WithFiles(_fullDockerComposeFilePath).Build();
            await upProjectCommand.Execute(TimeSpan.FromSeconds(10));
        }

        private string _fullDockerComposeFilePath;

        private const int ServerPort = 16379;
        private const string ServerHost = "localhost";
    }
}
