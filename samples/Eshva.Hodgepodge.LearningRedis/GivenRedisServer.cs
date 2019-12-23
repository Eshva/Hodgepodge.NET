#region Usings

using System.Threading.Tasks;
using FluentAssertions;
using StackExchange.Redis;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.LearningRedis
{
    [Collection(LearningRedisTestCollection.CollectionName)]
    public sealed class GivenRedisServer
    {
        public GivenRedisServer(LearningRedisTestCollectionFixture collectionFixture)
        {
            Redis = collectionFixture.Redis;
            Server = collectionFixture.Server;
        }

        [Fact]
        public async Task ShouldFlushAllDatabases()
        {
            var database0 = Redis.GetDatabase(0);
            var database1 = Redis.GetDatabase(1);

            const string Key = "eshva-hodgepodge:test1";
            await database0.StringSetAsync(Key, "value0");
            (await database0.StringGetAsync(Key)).Should().Be("value0");
            await database1.StringSetAsync(Key, "value1");
            (await database1.StringGetAsync(Key)).Should().Be("value1");

            await Server.FlushAllDatabasesAsync();

            (await database0.KeyExistsAsync(Key)).Should().BeFalse();
            (await database1.KeyExistsAsync(Key)).Should().BeFalse();
        }

        private ConnectionMultiplexer Redis { get; }

        private IServer Server { get; }
    }
}
