#region Usings

using System;
using Eshva.DockerCompose.Commands.RestartServices;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.RestartServices
{
    public sealed class GivenRestartServicesCommandWhenBuildingCommandAndSettingOptions
    {
        [Fact]
        public void ShouldBuildCommandThatRestartsAllServicesInProject()
        {
            TestOption(
                builder => builder.RestartAllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" restart", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatRestartsSomeServicesInProject()
        {
            TestOption(
                builder => builder.RestartServices("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatHasShutdownTimeout()
        {
            TestOption(
                builder => builder.RestartServices("service1").ShutdownTimeoutSeconds(111),
                arguments => arguments.EndsWith("--timeout 111 service1", StringComparison.OrdinalIgnoreCase));
        }

        private static void TestOption(
            Func<RestartServicesCommandBuilder, RestartServicesCommandBuilder> configure,
            Func<string, bool> checkArguments)
        {
            var starterMock = new Mock<IProcessStarter>();
            var builder = RestartServicesCommand.WithFilesAndStarter(starterMock.Object, "file1", "file2");
            var command = configure(builder).Build();
            command.Execute();
            starterMock.Verify(
                starter => starter.Start(
                    It.Is<string>(arguments => checkArguments(arguments)),
                    TimeSpan.FromDays(1)),
                Times.Once());
        }
    }
}
