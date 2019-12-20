#region Usings

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.Tests.Unit.CollectionFixtureBase
{
    [Collection(CollectionFixtureBaseCollection.CollectionName)]
    public sealed class GivenCollectionFixtureBaseWhenDisposeAsyncCalled
    {
        [Fact]
        public async Task ShouldRemoveTestCollectionTemporaryFolder()
        {
            var collectionsFolder = Path.Combine(Path.GetTempPath(), "hodgepodge", "configs");
            Directory.Delete(collectionsFolder, true);
            await Task.Delay(TimeSpan.FromMilliseconds(10)); // NOTE: Removing from file system isn't synchronous.

            var collectionFixture = new InvocationLoggingCollectionFixture() as IAsyncLifetime;
            await collectionFixture.InitializeAsync();
            await Task.Delay(TimeSpan.FromMilliseconds(10)); // NOTE: Folder creating isn't synchronous.

            await collectionFixture.DisposeAsync();
            await Task.Delay(TimeSpan.FromMilliseconds(10)); // NOTE: Removing from file system isn't synchronous.

            Directory.GetDirectories(collectionsFolder).Length.Should().Be(0);
        }

        [Fact]
        public async Task ShouldInvokeMethodsInProperOrder()
        {
            var collectionFixture = new InvocationLoggingCollectionFixture();
            await ((IAsyncLifetime)collectionFixture).InitializeAsync();
            collectionFixture.InvocationOrder.Clear();

            await ((IAsyncLifetime)collectionFixture).DisposeAsync();

            collectionFixture.InvocationOrder.Should().BeEquivalentTo("TeardownCollection");
        }
    }
}
