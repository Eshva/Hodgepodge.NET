#region Usings

using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eshva.Polls.Admin.Tests.EndToEnd.TestHelpers;
using FluentAssertions;
using StackExchange.Redis;
using Xunit;

#endregion


namespace Eshva.Polls.Admin.Tests.EndToEnd.Features
{
    [Collection(PollsTestCollection.CollectionName)]
    public sealed class SetClientConfigurationFeature
    {
        public SetClientConfigurationFeature(PollsTestCollectionFixture collectionFixture)
        {
            _database = collectionFixture.Redis.GetDatabase();
            _httpClient = collectionFixture.HttpClient;
            _pollsAdminBffUri = new Uri(collectionFixture.PollsAdminBffUri, "/api/v1/configs");
        }

        [Fact]
        public async Task ShouldSetClientConfiguration()
        {
            var requestUri = new Uri(_pollsAdminBffUri, "client");
            var expected = new Random().Next();
            var clientConfiguration = new ClientConfig { ConfigurationRefreshIntervalSeconds = expected };
            var content = new StringContent(JsonSerializer.Serialize(clientConfiguration), Encoding.UTF8, MediaTypeNames.Application.Json);
            await _httpClient.PostAsync(requestUri, content);

            var value = JsonSerializer.Deserialize<ClientConfig>(await _database.StringGetAsync("config:client"));
            value.ConfigurationRefreshIntervalSeconds.Should().Be(expected);
        }

        [Fact]
        public async Task SmokeTest()
        {
            await _database.StringSetAsync("qu", "kva");
            var value = await _database.StringGetAsync("qu");
            value.Should().Be("kva");
        }

        private readonly HttpClient _httpClient;
        private readonly Uri _pollsAdminBffUri;
        private readonly IDatabase _database;

        private sealed class ClientConfig
        {
            public int ConfigurationRefreshIntervalSeconds { get; set; }
        }
    }
}
