#region Usings

using Xunit;

#endregion


namespace Eshva.Hodgepodge.Tests.Unit.CollectionFixtureBase
{
    [CollectionDefinition(CollectionName)]
    public sealed class CollectionFixtureBaseCollection : ICollectionFixture<CollectionFixtureBaseCollectionFixture>
    {
        public const string CollectionName = "CollectionFixtureBase test collection";
    }
}
