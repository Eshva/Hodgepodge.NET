#region Usings

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.UpProjectCommand
{
    public sealed class GivenUpProjectCommandWhenExecuteCalled
    {
        [Fact]
        public async Task ShouldContainValidArgumentsIfDetached()
        {
            var starterMock = new Mock<IProcessStarter>();
            Expression<Func<string, bool>> argumentsValidator =
                arguments => arguments.Equals("-f \"project.yaml\" up --detach", StringComparison.OrdinalIgnoreCase);
            starterMock.Setup(starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = new Commands.UpProject.UpProjectCommand(starterMock.Object, "project.yaml")
                          {
                              IsDetached = true
                          };
            await command.Execute();
            starterMock.Verify(
                starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()),
                Times.Once());
        }
    }
}
