#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;

#endregion


namespace Eshva.DockerCompose.Commands.UpProject
{
    public sealed class UpProjectCommand : CommandBase
    {
        private UpProjectCommand(IProcessStarter processStarter, params string[] files) : base(processStarter, files)
        {
        }

        private UpProjectCommand(params string[] files) : base(files)
        {
        }

        internal bool IsDetached { private get; set; } = true;

        public static UpProjectCommandBuilder WithFiles(params string[] files) =>
            new UpProjectCommandBuilder(new UpProjectCommand(files));

        public static UpProjectCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new UpProjectCommandBuilder(new UpProjectCommand(starter, files));

        protected internal override string[] Verify()
        {
            // TODO: Verification.
            return new string[] { };
        }

        protected override string Command => "up";

        protected override IReadOnlyCollection<string> PrepareArguments()
        {
            var arguments = new List<string> { "up" };
            arguments.AddConditionally(IsDetached, "--detach");
            return arguments.AsReadOnly();
        }
    }
}
