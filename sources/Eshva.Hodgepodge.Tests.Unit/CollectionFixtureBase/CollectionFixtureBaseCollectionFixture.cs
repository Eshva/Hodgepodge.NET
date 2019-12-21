#region Usings

using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.Tests.Unit.CollectionFixtureBase
{
    [Collection(CollectionFixtureBaseCollection.CollectionName)]
    [UsedImplicitly]
    public sealed class CollectionFixtureBaseCollectionFixture : IAsyncLifetime
    {
        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
