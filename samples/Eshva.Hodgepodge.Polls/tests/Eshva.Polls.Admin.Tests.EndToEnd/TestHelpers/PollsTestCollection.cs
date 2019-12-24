#region Usings

using Xunit;

#endregion


namespace Eshva.Polls.Admin.Tests.EndToEnd.TestHelpers
{
    [CollectionDefinition(CollectionName)]
    public sealed class PollsTestCollection : ICollectionFixture<PollsTestCollectionFixture>
    {
        public const string CollectionName = "Polls admin collection";
    }
}
