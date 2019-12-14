#region Usings

using System;
using Eshva.DockerCompose.Commands;
using Eshva.DockerCompose.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands
{
    public sealed class GivenCommandBuilderBaseWhenBuildCalled
    {
        [Fact]
        public void ShouldVerifyCommand()
        {
            var commandMock = new Mock<CommandBase>();
            var builder = new TestCommandBuilder(commandMock.Object);

            builder.Build();

            commandMock.Verify(command => command.Verify(), Times.Once());
        }

        [Fact]
        public void ShouldThrowWithErrorsIfAnyFound()
        {
            var commandMock = new Mock<CommandBase>();
            commandMock.Setup(command => command.Verify()).Returns(new[] { "error1", "error2" });
            var builder = new TestCommandBuilder(commandMock.Object);

            Action build = () => builder.Build();

            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain("error1").And.Contain("error2");
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Message.Should().Contain("error1").And.Contain("error2");
        }

        [Fact]
        public void ShouldReturnBuiltCommandIfNoVerificationErrorsFound()
        {
            var commandMock = new Mock<CommandBase>();
            commandMock.Setup(command => command.Verify()).Returns(new string[0]);
            var builder = new TestCommandBuilder(commandMock.Object);

            Func<CommandBase> build = () => builder.Build();

            build.Should().NotThrow().Which.Should().BeSameAs(commandMock.Object);
        }

        private sealed class TestCommandBuilder : CommandBuilderBase<CommandBase>
        {
            public TestCommandBuilder(CommandBase command) : base(command)
            {
            }
        }
    }
}
