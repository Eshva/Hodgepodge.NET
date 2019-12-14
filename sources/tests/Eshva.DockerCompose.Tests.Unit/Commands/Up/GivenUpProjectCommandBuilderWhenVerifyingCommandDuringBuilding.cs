#region Usings

using System;
using Eshva.DockerCompose.Commands.UpProject;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Up
{
    public sealed class GivenUpProjectCommandBuilderWhenVerifyingCommandDuringBuilding
    {
        [Fact]
        public void ShouldDisallowCombiningOfDetachingAndStoppingAllContainersIfAnyOneStopped()
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = UpProjectCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            Action build = () => builder.StopAllContainersIfAnyOneStopped().Build();
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain(
                     error => error.Contains("detach", StringComparison.OrdinalIgnoreCase) &&
                              error.Contains("stop", StringComparison.OrdinalIgnoreCase));
        }

        private static void TestOption(
            Func<UpProjectCommandBuilder, UpProjectCommandBuilder> configure,
            Func<string, bool> checkArguments)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = UpProjectCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            var command = configure(builder).Build();
            command.Execute();
            processStarterMock.Verify(
                starter => starter.Start(
                    It.Is<string>(arguments => checkArguments(arguments)),
                    TimeSpan.FromDays(1)),
                Times.Once());
        }
    }
}
