#region Usings

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

#endregion


namespace Eshva.Hodgepodge.Tests.Unit.CollectionFixtureBase
{
    [Collection(CollectionFixtureBaseCollection.CollectionName)]
    public sealed class GivenCollectionFixtureBaseWhenInitializeAsyncCalled
    {
        [Fact]
        public async Task ShouldCreateTemporaryFolderForTestCollection()
        {
            var collectionsFolder = Path.Combine(Path.GetTempPath(), "hodgepodge", "configs");
            Directory.Delete(collectionsFolder, true);
            await Task.Delay(TimeSpan.FromMilliseconds(10)); // NOTE: Removing from file system isn't synchronous.

            var collectionFixture = new InvocationLoggingCollectionFixture() as IAsyncLifetime;
            await collectionFixture.InitializeAsync();

            await Task.Delay(TimeSpan.FromMilliseconds(10)); // NOTE: Folder creating isn't synchronous.
            Directory.GetDirectories(collectionsFolder).Length.Should().Be(1);
        }

        [Fact]
        public async Task ShouldInvokeMethodsInProperOrder()
        {
            var collectionFixture = new InvocationLoggingCollectionFixture();
            await ((IAsyncLifetime)collectionFixture).InitializeAsync();

            collectionFixture.InvocationOrder.Should().BeEquivalentTo(
                "SetupCollection",
                "GetPreparationTimeout",
                "WaitCollectionReady",
                "PrepareFixture");
        }

        [Fact]
        public async Task ShouldStopWaitingForCollectionReadinessIfTimeoutPassed()
        {
            const int TimeoutMilliseconds = 100;
            var collectionFixture = new TimeoutOverrunCollectionFixture(
                TimeSpan.FromMilliseconds(TimeoutMilliseconds),
                TimeSpan.FromMilliseconds(TimeoutMilliseconds * 5)) as IAsyncLifetime;

            var stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                await collectionFixture.InitializeAsync();
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                stopwatch.Stop();
            }

            stopwatch.Elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(TimeoutMilliseconds), TimeSpan.FromMilliseconds(20));
        }

        private sealed class TimeoutOverrunCollectionFixture : Hodgepodge.CollectionFixtureBase
        {
            public TimeoutOverrunCollectionFixture(TimeSpan timeout, TimeSpan delay)
            {
                _timeout = timeout;
                _delay = delay;
            }

            protected override Task SetupCollection() => Task.CompletedTask;

            protected override Task TeardownCollection() => Task.CompletedTask;

            protected override async Task WaitCollectionReady(CancellationToken cancellationToken)
            {
                await Task.Delay(_delay, cancellationToken);
            }

            protected override Task PrepareFixture() => Task.CompletedTask;

            protected override TimeSpan GetPreparationTimeout() => _timeout;

            private readonly TimeSpan _delay;
            private readonly TimeSpan _timeout;
        }
    }
}
