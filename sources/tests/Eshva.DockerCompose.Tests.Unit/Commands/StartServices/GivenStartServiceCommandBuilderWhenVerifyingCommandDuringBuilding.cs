#region Usings

using System;
using Eshva.DockerCompose.Commands.StartServices;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.StartServices
{
    public sealed class GivenStartServiceCommandBuilderWhenVerifyingCommandDuringBuilding
    {
        [Fact]
        public void ShouldDisallowCombiningStartOfAllAndSomeServices()
        {
            ValidateOptions(
                builder => builder.StartAllServices().StartServices("service1", "service2", "service3"),
                error => error.Equals(
                    "It's not allowed to configure all services and specify to start some of them. " +
                    "StartAllServices and StartServices shouldn't be used together.",
                    StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldRequireStartAllServicesOrSomeOfThem()
        {
            ValidateOptions(
                builder => builder,
                error => error.Equals(
                    "You should specify to start all services or some of them. " +
                    "Use StartAllServices or StartServices method.",
                    StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidateOptions(
            Func<StartServicesCommandBuilder, StartServicesCommandBuilder> configure,
            Func<string, bool> validateOptions)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = StartServicesCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            Action build = () => configure(builder).Build();
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain(error => validateOptions(error));
        }
    }
}
