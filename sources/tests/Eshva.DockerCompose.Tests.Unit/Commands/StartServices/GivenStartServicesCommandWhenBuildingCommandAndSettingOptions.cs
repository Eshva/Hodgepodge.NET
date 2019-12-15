#region Usings

using System;
using Eshva.DockerCompose.Commands.StartServices;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.StartServices
{
    public sealed class GivenStartServicesCommandWhenBuildingCommandAndSettingOptions
    {
        [Fact]
        public void ShouldBuildCommandThatStartsAllServicesInProject()
        {
            TestOption(
                builder => builder.StartAllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" start", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatStartsSomeServicesInProject()
        {
            TestOption(
                builder => builder.StartServices("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        private static void TestOption(
            Func<StartServicesCommandBuilder, StartServicesCommandBuilder> configure,
            Func<string, bool> checkArguments)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = StartServicesCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
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
