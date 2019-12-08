#region Usings

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eshva.DockerCompose.Commands;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit
{
    public sealed class GivenCommandBaseWhenExecuteCalled
    {
        [Fact]
        public void ShouldExecuteDockerComposeWithNoArgumentsWithoutExceptions()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = new VersionCommand(starterMock.Object, string.Empty);
            Func<Task> execute = async () => await command.Execute();
            execute.Should().NotThrow(
                "При условии установленного Docker Compose получение его версии всегда допустим.");
        }

        [Fact]
        public void ShouldThrowIfBadCommandExecuted()
        {
            var command = new BadCommand();
            Func<Task> execute = async () => await command.Execute();
            execute.Should().ThrowExactly<CommandExecutionException>();
        }

        [Fact]
        public async Task ShouldStartWithProperArguments()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start("--version", It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = new VersionCommand(starterMock.Object, string.Empty);
            await command.Execute();
            starterMock.Verify(starter => starter.Start("--version", It.IsAny<TimeSpan>()), Times.Once());
        }

        [Fact]
        public async Task ShouldStartWithProperTimeout()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start(It.IsAny<string>(), TimeSpan.FromHours(1)))
                       .Returns(Task.FromResult(0));
            var command = new VersionCommand(starterMock.Object, string.Empty);
            await command.Execute(TimeSpan.FromHours(1));
            starterMock.Verify(starter => starter.Start(It.IsAny<string>(), TimeSpan.FromHours(1)), Times.Once());
        }

        [Fact]
        public async Task ShouldStartWithAllProjectFilesIncluded()
        {
            var starterMock = new Mock<IProcessStarter>();
            Expression<Func<string, bool>> argumentsValidator =
                arguments => arguments.Contains("-f \"project1.yaml\"") && arguments.Contains("-f \"project2.yaml\"");
            starterMock.Setup(
                           starter => starter.Start(It.Is(argumentsValidator), TimeSpan.FromHours(1)))
                       .Returns(Task.FromResult(0));
            var command = new CommandWithProjects(starterMock.Object, "project1.yaml", "project2.yaml");
            await command.Execute(TimeSpan.FromHours(1));
            starterMock.Verify(
                starter => starter.Start(It.Is(argumentsValidator), TimeSpan.FromHours(1)),
                Times.Once());
        }

        private sealed class BadCommand : CommandBase
        {
            protected override IReadOnlyCollection<string> PrepareArguments() =>
                new List<string>(new[] { "fook", "-it" }).AsReadOnly();
        }

        private sealed class CommandWithProjects : CommandBase
        {
            public CommandWithProjects(IProcessStarter processStarter, params string[] projectFileNames)
                : base(processStarter, projectFileNames)
            {
            }

            protected override IReadOnlyCollection<string> PrepareArguments() => new List<string>().AsReadOnly();
        }

        private sealed class VersionCommand : CommandBase
        {
            public VersionCommand(IProcessStarter processStarter, params string[] projectFileNames)
                : base(processStarter, projectFileNames)
            {
            }

            protected override IReadOnlyCollection<string> PrepareArguments() =>
                new List<string>(new[] { "--version" }).AsReadOnly();
        }
    }
}
