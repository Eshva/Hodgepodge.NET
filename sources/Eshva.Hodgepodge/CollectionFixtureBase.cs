#region Usings

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

#endregion


namespace Eshva.Hodgepodge
{
    public abstract class CollectionFixtureBase : IAsyncLifetime
    {
        async Task IAsyncLifetime.InitializeAsync()
        {
            CreateTestCollectionFolder();
            await SetupCollection();
            await WaitCollectionReady(new CancellationTokenSource(GetPreparationTimeout()).Token);
            await PrepareFixture();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await TeardownCollection();
            DeleteTestCollectionFolder();
        }

        protected string CollectionFolder { get; private set; }

        protected abstract Task SetupCollection();

        protected abstract Task TeardownCollection();

        protected abstract Task WaitCollectionReady(CancellationToken cancellationToken);

        protected virtual TimeSpan GetPreparationTimeout() => TimeSpan.FromSeconds(10);

        protected abstract Task PrepareFixture();

        private void CreateTestCollectionFolder()
        {
            CollectionFolder = Path.Combine(Path.GetTempPath(), "hodgepodge", "configs", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(CollectionFolder);
        }

        private void DeleteTestCollectionFolder()
        {
            Directory.Delete(CollectionFolder, true);
        }
    }
}
