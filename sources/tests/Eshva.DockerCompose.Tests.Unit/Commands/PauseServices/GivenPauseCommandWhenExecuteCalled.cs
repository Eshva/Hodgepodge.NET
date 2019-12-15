#region Usings

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eshva.DockerCompose.Commands.PauseServices;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Pause
{
    public sealed class GivenPauseCommandWhenExecuteCalled
    {
        [Fact]
        public async Task ShouldContainValidArguments()
        {
            var starterMock = new Mock<IProcessStarter>();
            Expression<Func<string, bool>> argumentsValidator =
                arguments => arguments.Equals("-f \"project.yaml\" pause", StringComparison.OrdinalIgnoreCase);
            starterMock.Setup(starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = PauseServicesCommand.WithFilesAndStarter(starterMock.Object, "project.yaml")
                                      .AllServices()
                                      .Build();
            await command.Execute();
            starterMock.Verify(
                starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()),
                Times.Once());
        }
    }
}
