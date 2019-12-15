#region Usings

using System;
using Eshva.DockerCompose.Commands.Logs;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Logs
{
    public sealed class GivenLogsCommandWhenBuildingCommandAndSettingOptionsCalled
    {
        [Fact]
        public void ShouldBuildCommandThatTakesLogsFromAllServicesInProject()
        {
            TestOption(
                builder => builder.FromAllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" logs", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatStartsSomeServicesInProject()
        {
            TestOption(
                builder => builder.FromServices("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandWithServiceListInTheEnd()
        {
            TestOption(
                builder => builder
                           .FromServices("service1", "service2", "service3")
                           .WithTimestamps()
                           .TakeNumberOfLines(111),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatOutputsTimestamps()
        {
            TestOption(
                builder => builder.FromAllServices().WithTimestamps(),
                arguments => arguments.Contains("--timestamps"));
        }

        [Fact]
        public void ShouldBuildCommandThatTakesOnlyLimitedNumberOfLogLines()
        {
            TestOption(
                builder => builder.FromAllServices().TakeNumberOfLines(111),
                arguments => arguments.Contains("--tail 111"));
        }

        private static void TestOption(
            Func<LogsCommandBuilder, LogsCommandBuilder> configure,
            Func<string, bool> checkArguments)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = LogsCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
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
