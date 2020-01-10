#region Usings

using System.Threading;
using System.Threading.Tasks;
using Eshva.Polls.Admin.Application.SetClientConfiguration;
using Eshva.Polls.Admin.WebApp.Resources.ClientConfiguration;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

#endregion


namespace Eshva.Polls.Admin.WebApp.Tests.Unit
{
    public sealed class GivenClientConfigurationControllerWhenSetCalled
    {
        [Fact]
        public async Task ShouldReturnOkStatusForValidConfigurationObject()
        {
            var mediatorMock = new Mock<IMediator>();

            var controller = new ClientConfigurationController(mediatorMock.Object);
            var clientConfiguration = new SetClientConfigurationRequest { ConfigurationRefreshIntervalSeconds = 111 };
            var result = await controller.Set(clientConfiguration);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ShouldDelegateConfigurationObjectToMediator()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<SetClientConfigurationRequest>(), CancellationToken.None))
                .Returns(MediatR.Unit.Task);

            var controller = new ClientConfigurationController(mediatorMock.Object);
            var clientConfiguration = new SetClientConfigurationRequest { ConfigurationRefreshIntervalSeconds = 111 };
            await controller.Set(clientConfiguration);

            mediatorMock.Verify(mediator => mediator.Send(It.IsAny<SetClientConfigurationRequest>(), CancellationToken.None), Times.Once());
        }
    }
}
