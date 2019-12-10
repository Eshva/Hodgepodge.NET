#region Usings

using Eshva.DockerCompose.Commands.UpProject;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands
{
    public class GivenUpProjectCommandBuilder
    {
        [Fact]
        public void ShouldTBD()
        {
            var command = UpProjectCommand.WithFiles("sdfsd", "sfsdfsd")
                                          .Detached()
                                          .Build();
        }
    }
}
