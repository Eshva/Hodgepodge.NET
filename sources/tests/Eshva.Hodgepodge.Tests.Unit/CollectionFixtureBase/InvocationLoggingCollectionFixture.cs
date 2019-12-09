#region Usings

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion


namespace Eshva.Hodgepodge.Tests.Unit.CollectionFixtureBase
{
    internal sealed class InvocationLoggingCollectionFixture : Hodgepodge.CollectionFixtureBase
    {
        public List<string> InvocationOrder { get; } = new List<string>();

        protected override Task SetupCollection()
        {
            InvocationOrder.Add("SetupCollection");
            return Task.CompletedTask;
        }

        protected override Task TeardownCollection()
        {
            InvocationOrder.Add("TeardownCollection");
            return Task.CompletedTask;
        }

        protected override Task WaitCollectionReady(CancellationToken cancellationToken)
        {
            InvocationOrder.Add("WaitCollectionReady");
            return Task.CompletedTask;
        }

        protected override Task PrepareFixture()
        {
            InvocationOrder.Add("PrepareFixture");
            return Task.CompletedTask;
        }

        protected override TimeSpan GetPreparationTimeout()
        {
            InvocationOrder.Add("GetPreparationTimeout");
            return base.GetPreparationTimeout();
        }
    }
}
