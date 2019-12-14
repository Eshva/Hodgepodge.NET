#region Usings

using Eshva.DockerCompose.Commands.UpProject;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Up
{
    public sealed class GivenUpProjectCommandBuilder
    {
        [Fact]
        public void ShouldTBD()
        {
            var command = UpProjectCommand.WithFiles("sdfsd", "sfsdfsd")
                                          .Build();
        }
    }
}
