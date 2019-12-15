#region Usings

using System;
using Eshva.DockerCompose.Commands.StopServices;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.StopServices
{
    public sealed class GivenStopServicesCommandWhenBuildingCommandAndSettingOptions
    {
        [Fact]
        public void ShouldBuildCommandThatStopsAllServicesInProject()
        {
            TestOption(
                builder => builder.StopAllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" stop", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatStopsSomeServicesInProject()
        {
            TestOption(
                builder => builder.StopServices("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatHasShutdownTimeout()
        {
            TestOption(
                builder => builder.StopServices("service1").ShutdownTimeoutSeconds(111),
                arguments => arguments.EndsWith("--timeout 111 service1", StringComparison.OrdinalIgnoreCase));
        }


        private static void TestOption(
            Func<StopServicesCommandBuilder, StopServicesCommandBuilder> configure,
            Func<string, bool> checkArguments)
        {
            var starterMock = new Mock<IProcessStarter>();
            var builder = StopServicesCommand.WithFilesAndStarter(starterMock.Object, "file1", "file2");
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
