using Xunit;


namespace Eshva.Polls.Admin.Tests.EndToEnd.TestHelpers
{
    [CollectionDefinition(nameof(UC005TestCollection), DisableParallelization = true)]
    public sealed class UC005TestCollection : ICollectionFixture<UC005TestCollectionFixture>
    {
    }
}