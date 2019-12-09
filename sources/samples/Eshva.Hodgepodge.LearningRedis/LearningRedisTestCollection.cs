#region Usings

using Xunit;

#endregion


namespace Eshva.Hodgepodge.LearningRedis
{
    [CollectionDefinition(CollectionName)]
    public sealed class LearningRedisTestCollection : ICollectionFixture<LearningRedisTestCollectionFixture>
    {
        public const string CollectionName = "Learning Redis";
    }
}
