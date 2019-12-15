#region Usings

using System;
using Eshva.DockerCompose.Commands.Logs;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Logs
{
    public sealed class GivenLogsCommandBuilderWhenVerifyingCommandDuringBuilding
    {
        [Fact]
        public void ShouldDisallowCombiningTakeLogsFromAllAndSomeServices()
        {
            ValidateOptions(
                builder => builder.FromAllServices().FromServices("service1", "service2", "service3"),
                error => error.Equals(
                    "It's not allowed to configure all services and specify to start some of them. " +
                    "FromAllServices and FromServices shouldn't be used together.",
                    StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldRequireTakingLogsFromAllServicesOrSomeOfThem()
        {
            ValidateOptions(
                builder => builder,
                error => error.Equals(
                    "You should specify to take logs from all services or from some of them. " +
                    "Use FromAllServices or FromServices method.",
                    StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidateOptions(
            Func<LogsCommandBuilder, LogsCommandBuilder> configure,
            Func<string, bool> validateOptions)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = LogsCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            Action build = () => configure(builder).Build();
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain(error => validateOptions(error));
        }
    }
}
