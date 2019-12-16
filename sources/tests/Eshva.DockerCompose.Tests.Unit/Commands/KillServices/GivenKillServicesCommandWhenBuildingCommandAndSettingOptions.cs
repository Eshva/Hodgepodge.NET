#region Usings

using System;
using Eshva.DockerCompose.Commands.KillServices;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.KillServices
{
    public sealed class GivenKillServicesCommandWhenBuildingCommandAndSettingOptions
    {
        [Fact]
        public void ShouldBuildCommandThatKillsAllServicesInProject()
        {
            TestOption(
                builder => builder.AllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" kill", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatKillsSomeServicesInProject()
        {
            TestOption(
                builder => builder.Services("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatSendsCustomSignalToServices()
        {
            TestOption(
                builder => builder.AllServices().WithSignal("SIGINT"),
                arguments => arguments.Contains("-s SIGINT"));
        }

        private static void TestOption(
            Func<KillServicesCommandBuilder, KillServicesCommandBuilder> configure,
            Func<string, bool> checkArguments)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = KillServicesCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
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
