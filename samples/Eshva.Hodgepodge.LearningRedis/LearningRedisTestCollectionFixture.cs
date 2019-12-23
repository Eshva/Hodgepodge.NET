#region Usings

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Eshva.DockerCompose.Commands.DownProject;
using Eshva.DockerCompose.Commands.UpProject;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.LearningRedis
{
    [UsedImplicitly]
    [Collection(LearningRedisTestCollection.CollectionName)]
    public sealed class LearningRedisTestCollectionFixture : CollectionFixtureBase
    {
        public ConnectionMultiplexer Redis { get; private set; }

        public IServer Server { get; private set; }

        protected override async Task SetupCollection()
        {
            SaveEmbeddedFilesToTempFolder("docker-configs/learning-redis.yaml");
            await UpDockerComposeFile("learning-redis.yaml");
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

        private void SaveEmbeddedFilesToTempFolder(params string[] embeddedFileNames)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var embeddedFileProvider = new ManifestEmbeddedFileProvider(executingAssembly);

            foreach (var embeddedFileName in embeddedFileNames)
            {
                using var reader = embeddedFileProvider.GetFileInfo(embeddedFileName).CreateReadStream();
                using var writer = new StreamWriter(Path.Combine(CollectionFolder, Path.GetFileName(embeddedFileName)));
                reader.CopyTo(writer.BaseStream);
            }
        }

        private async Task UpDockerComposeFile(string dockerComposeFileName)
        {
            _fullDockerComposeFilePath = Path.Combine(CollectionFolder, dockerComposeFileName);
            //var upProjectCommand = new UpProjectCommand(_fullDockerComposeFilePath);
            var upProjectCommand = UpProjectCommand.WithFiles(_fullDockerComposeFilePath).Build();
            await upProjectCommand.Execute(TimeSpan.FromSeconds(10));
        }

        private const int ServerPort = 16379;
        private const string ServerHost = "localhost";

        private string _fullDockerComposeFilePath;
    }
}
