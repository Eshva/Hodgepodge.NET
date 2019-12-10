#region Usings

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands
{
    public sealed class GivenDownProjectCommandWhenExecuteCalled
    {
        [Fact]
        public async Task ShouldContainValidArguments()
        {
            var starterMock = new Mock<IProcessStarter>();
            Expression<Func<string, bool>> argumentsValidator =
                arguments => arguments.Equals("-f \"project.yaml\" down", StringComparison.OrdinalIgnoreCase);
            starterMock.Setup(starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = new DockerCompose.Commands.DownProject.DownProjectCommand(starterMock.Object, "project.yaml");
            await command.Execute();
            starterMock.Verify(
                starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()),
                Times.Once());
        }
    }
}
