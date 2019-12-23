#region Usings

using System.Text.Json;
using System.Threading.Tasks;
using Eshva.Polls.Admin.Tests.EndToEnd.TestHelpers;
using Eshva.Polls.Configuration.Domain;
using FluentAssertions;
using StackExchange.Redis;
using TechTalk.SpecFlow;

#endregion


namespace Eshva.Polls.Admin.Tests.EndToEnd.StepDefinitions
{
    [Binding]
    public class UC005_SetClientConfigurationSteps
    {
        public UC005_SetClientConfigurationSteps(UC005TestCollectionFixture collectionFixture)
        {
            _redis = collectionFixture.Redis;
        }

        [Given(@"administrator open Client configuration page")]
        public void GivenAdministratorOpenClientConfigurationPage()
        {
        }

        [When(@"administrator changed the configuration refresh interval")]
        public void WhenAdministratorChangedTheConfigurationRefreshInterval()
        {
        }

        [When(@"commands to store client configuration")]
        public void WhenCommandsToStoreClientConfiguration()
        {
        }

        [Then(@"the configuration refresh interval should be changed in the product configuration database")]
        public async Task ThenTheConfigurationRefreshIntervalShouldBeChangedInTheProductConfigurationDatabase()
        {
            var database = _redis.GetDatabase();
            var configsClientString = await database.StringGetAsync("configs:client");
            var clientConfiguration = JsonSerializer.Deserialize<ClientConfiguration>(configsClientString.ToString());
            clientConfiguration.ConfigurationRefreshIntervalSeconds.Should().Be(111);
        }

        private readonly ConnectionMultiplexer _redis;
    }
}
