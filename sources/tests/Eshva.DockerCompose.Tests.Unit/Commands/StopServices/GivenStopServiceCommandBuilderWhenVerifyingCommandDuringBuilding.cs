#region Usings

using System;
using Eshva.DockerCompose.Commands.StopServices;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.StopServices
{
    public sealed class GivenStopServiceCommandBuilderWhenVerifyingCommandDuringBuilding
    {
        [Fact]
        public void ShouldDisallowCombiningStopOfAllAndSomeServices()
        {
            ValidateOptions(
                builder => builder.StopAllServices().StopServices("service1", "service2", "service3"),
                error => error.Equals(
                    "It's not allowed to configure all services and specify to stop some of them. " +
                    "StopAllServices and StopServices shouldn't be used together.",
                    StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldRequireStopAllServicesOrSomeOfThem()
        {
            ValidateOptions(
                builder => builder,
                error => error.Equals(
                    "You should specify to stop all services or some of them. " +
                    "Use StopAllServices or StopServices method.",
                    StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidateOptions(
            Func<StopServicesCommandBuilder, StopServicesCommandBuilder> configure,
            Func<string, bool> validateOptions)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = StopServicesCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            Action build = () => configure(builder).Build();
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain(error => validateOptions(error));
        }
    }
}
