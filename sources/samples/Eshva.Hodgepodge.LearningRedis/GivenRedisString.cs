#region Usings

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Eshva.DockerCompose.Commands.DownProject;
using Eshva.DockerCompose.Commands.UpProject;
using FluentAssertions;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.LearningRedis
{
    public sealed class GivenRedisString : IAsyncLifetime
    {
        [Fact]
        public async Task ShouldWriteString()
        {
            var database = _redis.GetDatabase();
            var value = Guid.NewGuid().ToString("N");
            const string Key = "eshva-hodgepodge:test1";
            await database.StringSetAsync(Key, value);

            var readValue = await database.StringGetAsync(Key);
            readValue.Should().Be(readValue);
        }

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

        private Task GetRedisDatabase()
        {
            while (true)
            {
                try
                {
                    _redis = ConnectionMultiplexer.Connect("localhost:16379");
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

        private async Task RedisDown()
        {
            _redis?.Dispose();
            var downProjectCommand = new DownProjectCommand(_fullDockerComposeFilePath);
            await downProjectCommand.Execute();
        }

        private string _configurationTempFolder;
        private string _fullDockerComposeFilePath;
        private ConnectionMultiplexer _redis;
    }
}
