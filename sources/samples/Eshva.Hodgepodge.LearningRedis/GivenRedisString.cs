#region Usings

using System;
using System.Threading.Tasks;
using FluentAssertions;
using StackExchange.Redis;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.LearningRedis
{
    [Collection("Learning Redis")]
    public sealed class GivenRedisString
    {
        public GivenRedisString(LearningRedisFixture fixture)
        {
            Redis = fixture.Redis;
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
    }
}
