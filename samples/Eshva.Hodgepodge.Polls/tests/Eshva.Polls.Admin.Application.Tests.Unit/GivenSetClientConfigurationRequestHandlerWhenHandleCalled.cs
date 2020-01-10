#region Usings

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Eshva.Polls.Admin.Application.SetClientConfiguration;
using Eshva.Polls.Configuration.Contracts.Client;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.Polls.Admin.Application.Tests.Unit
{
    public sealed class GivenSetClientConfigurationRequestHandlerWhenHandleCalled
    {
        [Fact]
        public async Task ShouldDelegateWorkToClientConfigurationService()
        {
            const int ConfigurationRefreshIntervalSeconds = 111;
            var serviceMock = new Mock<IClientConfigurationService>(MockBehavior.Strict);
            serviceMock.Setup(
                           service => service.SetClientConfiguration(
                               It.Is<ClientConfiguration>(
                                   configuration =>
                                       configuration.ConfigurationRefreshIntervalSeconds == ConfigurationRefreshIntervalSeconds)))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            var requestHandler = new SetClientConfigurationRequestHandler(serviceMock.Object);
            await requestHandler.Handle(
                new SetClientConfigurationRequest { ConfigurationRefreshIntervalSeconds = ConfigurationRefreshIntervalSeconds },
                CancellationToken.None);

            serviceMock.Verify();
        }

        [Fact]
        public void ShouldThrowIfClientConfigurationServiceThrownException()
        {
            var serviceMock = new Mock<IClientConfigurationService>(MockBehavior.Strict);
            serviceMock.Setup(
                           service => service.SetClientConfiguration(It.IsAny<ClientConfiguration>()))
                       .ThrowsAsync(new HttpRequestException())
                       .Verifiable();

            var requestHandler = new SetClientConfigurationRequestHandler(serviceMock.Object);
            Func<Task<MediatR.Unit>> handle = async () => await requestHandler.Handle(
                                                  new SetClientConfigurationRequest { ConfigurationRefreshIntervalSeconds = 111 },
                                                  CancellationToken.None);
            handle.Should().Throw<ApplicationException>();
            serviceMock.Verify();
        }
    }
}
