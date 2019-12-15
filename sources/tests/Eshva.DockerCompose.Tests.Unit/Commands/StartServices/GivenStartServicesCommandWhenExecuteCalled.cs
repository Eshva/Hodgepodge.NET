#region Usings

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eshva.DockerCompose.Commands.StartServices;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.StartServices
{
    public sealed class GivenStartServicesCommandWhenExecuteCalled
    {
        [Fact]
        public async Task ShouldContainValidArguments()
        {
            var starterMock = new Mock<IProcessStarter>();
            Expression<Func<string, bool>> argumentsValidator =
                arguments => arguments.Equals("-f \"project.yaml\" start", StringComparison.OrdinalIgnoreCase);
            starterMock.Setup(starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = StartServicesCommand.WithFilesAndStarter(starterMock.Object, "project.yaml")
                                              .StartAllServices()
                                              .Build();
            await command.Execute();
            starterMock.Verify(
                starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()),
                Times.Once());
        }
    }
}
