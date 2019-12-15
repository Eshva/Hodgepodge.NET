#region Usings

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eshva.DockerCompose.Commands.StopServices;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.StopServices
{
    public sealed class GivenStopServicesCommandWhenExecuteCalled
    {
        [Fact]
        public async Task ShouldContainValidArguments()
        {
            var starterMock = new Mock<IProcessStarter>();
            Expression<Func<string, bool>> argumentsValidator =
                arguments => arguments.Equals("-f \"project.yaml\" stop", StringComparison.OrdinalIgnoreCase);
            starterMock.Setup(starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = StopServicesCommand.WithFilesAndStarter(starterMock.Object, "project.yaml")
                                              .StopAllServices()
                                              .Build();
            await command.Execute();
            starterMock.Verify(
                starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()),
                Times.Once());
        }
    }
}
