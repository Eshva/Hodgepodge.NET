#region Usings

using System;
using System.Threading.Tasks;
using FluentAssertions;
using StackExchange.Redis;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.LearningRedis
{
    [Collection(LearningRedisTestCollection.CollectionName)]
    public sealed class GivenRedisDatabase
    {
        public GivenRedisDatabase(LearningRedisTestCollectionFixture collectionFixture)
        {
            Redis = collectionFixture.Redis;
            Server = collectionFixture.Server;
        }

        [Fact]
        public async Task ShouldWriteString()
        {
            var database = Redis.GetDatabase();
            var value = Guid.NewGuid().ToString("N");
            const string Key = "eshva-hodgepodge:test1";
            await database.StringSetAsync(Key, value);

            var readValue = await database.StringGetAsync(Key);
            readValue.Should().Be(readValue);
        }

        private ConnectionMultiplexer Redis { get; }

        private IServer Server { get; }
    }
}
