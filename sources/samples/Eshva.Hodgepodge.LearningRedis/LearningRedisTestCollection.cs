#region Usings

using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
    [CollectionDefinition("Learning Redis")]
    public sealed class LearningRedisTestCollection : ICollectionFixture<LearningRedisFixture>
    {
    }

    [UsedImplicitly]
    [Collection("Learning Redis")]
    public sealed class LearningRedisFixture : IAsyncLifetime
    {
        public ConnectionMultiplexer Redis { get; private set; }

        Task IAsyncLifetime.InitializeAsync()
        {
            return RedisUp();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return RedisDown();
        }

        private async Task RedisUp()
        {
            CreateTempFolder();
            SaveConfigsToTempFolder();
            await UpDockerComposeFile("learning-redis.yaml");
            await GetRedisDatabase();
        }

        private async Task RedisDown()
        {
            Redis?.Dispose();
            var downProjectCommand = new DownProjectCommand(_fullDockerComposeFilePath);
            await downProjectCommand.Execute();
        }

        private Task GetRedisDatabase()
        {
            while (true)
            {
                try
                {
                    Redis = ConnectionMultiplexer.Connect("localhost:16379");
                    return Task.CompletedTask;
                }
                catch
                {
                    Console.WriteLine("Waiting for Redis 1s...");
                    Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        private void CreateTempFolder()
        {
            _configurationTempFolder = Path.Combine(Path.GetTempPath(), "hodgepodge", "configs", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_configurationTempFolder);
        }

        private void SaveConfigsToTempFolder()
        {
            // TODO: .NET converts dashes to underscores in path but not all seams like. I should do something about it.
            SaveEmbeddedFilesToTempFolder("docker-configs/learning-redis.yaml");
        }

        private void SaveEmbeddedFilesToTempFolder(params string[] embeddedFileNames)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var embeddedFileProvider = new ManifestEmbeddedFileProvider(executingAssembly);
            var fileInfos = embeddedFileProvider.GetDirectoryContents("/").ToArray();
            foreach (var embeddedFileName in embeddedFileNames)
            {
                using var reader = embeddedFileProvider.GetFileInfo(embeddedFileName).CreateReadStream();
                using var writer = new StreamWriter(Path.Combine(_configurationTempFolder, Path.GetFileName(embeddedFileName)));
                reader.CopyTo(writer.BaseStream);
            }
        }

        private async Task UpDockerComposeFile(string dockerComposeFileName)
        {
            _fullDockerComposeFilePath = Path.Combine(_configurationTempFolder, dockerComposeFileName);
            var upProjectCommand = new UpProjectCommand(_fullDockerComposeFilePath);
            await upProjectCommand.Execute(TimeSpan.FromSeconds(10));
        }

        private string _configurationTempFolder;
        private string _fullDockerComposeFilePath;
    }
}
