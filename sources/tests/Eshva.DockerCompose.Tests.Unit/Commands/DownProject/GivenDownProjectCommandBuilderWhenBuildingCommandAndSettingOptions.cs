#region Usings

using System;
using Eshva.DockerCompose.Commands.DownProject;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.DownProject
{
    public sealed class GivenDownProjectCommandBuilderWhenBuildingCommandAndSettingOptions
    {
        [Fact]
        public void ShouldBuildCommandThatRemovesAllImages()
        {
            TestOption(
                builder => builder.RemoveAllImages(),
                arguments => arguments.Contains("--rmi all"));
        }

        [Fact]
        public void ShouldBuildCommandThatRemovesLocalImages()
        {
            TestOption(
                builder => builder.RemoveLocalImages(),
                arguments => arguments.Contains("--rmi local"));
        }

        [Fact]
        public void ShouldBuildCommandThatRemovesVolumes()
        {
            TestOption(
                builder => builder.RemoveVolumes(),
                arguments => arguments.Contains("--volumes"));
        }

        [Fact]
        public void ShouldBuildCommandThatRemovesOrphanContainers()
        {
            TestOption(
                builder => builder.RemoveOrphanContainers(),
                arguments => arguments.Contains("--remove-orphans"));
        }

        [Fact]
        public void ShouldBuildCommandThatHasShutdownTimeoutSeconds()
        {
            TestOption(
                builder => builder.ShutdownTimeoutSeconds(111),
                arguments => arguments.Contains("--timeout 111"));
        }

        private static void TestOption(
            Func<DownProjectCommandBuilder, DownProjectCommandBuilder> configure,
            Func<string, bool> checkArguments)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = DownProjectCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
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
