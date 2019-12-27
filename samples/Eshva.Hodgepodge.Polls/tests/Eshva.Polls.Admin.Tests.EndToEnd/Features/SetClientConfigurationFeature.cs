#region Usings

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
        }

        [Fact]
        public async Task SmokeTest()
        {
            await _database.StringSetAsync("qu", "kva");
            var value = await _database.StringGetAsync("qu");
            value.Should().Be("kva");
        }

        private readonly IDatabase _database;
    }
}
