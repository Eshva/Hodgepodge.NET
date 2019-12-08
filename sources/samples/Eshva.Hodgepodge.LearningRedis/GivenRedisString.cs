#region Usings

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.LearningRedis
{
    public sealed class GivenRedisString : IDisposable
    {
        [Fact]
        public async Task ShouldWriteString()
        {
            await RedisUp();
            var database = _redis.GetDatabase();
            var value = Guid.NewGuid().ToString("N");
            const string Key = "eshva-hodgepodge:test1";
            database.StringSet(Key, value);

            database.StringGet(Key).Should().Be(value);
        }

        public void Dispose()
        {
            RedisDown();
        }

        private async Task RedisUp()
        {
            CreateTempFolder();
            SaveConfigsToTempFolder();
            UpDockerComposeFile("learning-redis.yaml");
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
            SaveEmbeddedFilesToTempFolder("docker_configs/learning-redis.yaml");
        }

        private void SaveEmbeddedFilesToTempFolder(params string[] embeddedFileNames)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var embeddedFileProvider = new EmbeddedFileProvider(executingAssembly);
            var fileInfos = embeddedFileProvider.GetDirectoryContents("/").ToArray();
            foreach (var embeddedFileName in embeddedFileNames)
            {
                using var reader = embeddedFileProvider.GetFileInfo(embeddedFileName).CreateReadStream();
                using var writer = new StreamWriter(Path.Combine(_configurationTempFolder, Path.GetFileName(embeddedFileName)));
                reader.CopyTo(writer.BaseStream);
            }
        }

        private void UpDockerComposeFile(string dockerComposeFileName)
        {
            _fullDockerComposeFilePath = Path.Combine(_configurationTempFolder, dockerComposeFileName);
            var process = ExecuteDockerCompose($"-f {_fullDockerComposeFilePath} up -d");
            var output = process.StandardOutput;
            var error = process.StandardError;
        }

        private Process ExecuteDockerCompose(string arguments)
        {
            var processStartInfo = new ProcessStartInfo("docker-compose", arguments)
                                   {
                                       RedirectStandardOutput = true,
                                       RedirectStandardError = true,
                                       CreateNoWindow = false,
                                       UseShellExecute = false
                                   };
            return Process.Start(processStartInfo);
        }

        private void RedisDown()
        {
            _redis.Dispose();
            var process = ExecuteDockerCompose($"-f {_fullDockerComposeFilePath} down");
            process.WaitForExit();
            var output = process.StandardOutput;
            var error = process.StandardError;
        }

        private string _configurationTempFolder;
        private string _fullDockerComposeFilePath;
        private ConnectionMultiplexer _redis;
    }
}
