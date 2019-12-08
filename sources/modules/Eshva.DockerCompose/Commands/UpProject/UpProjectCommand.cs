#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;

#endregion


namespace Eshva.DockerCompose.Commands.UpProject
{
    public sealed class UpProjectCommand : CommandBase
    {
        public UpProjectCommand(IProcessStarter processStarter, params string[] projectFileNames)
            : base(processStarter, projectFileNames)
        {
        }

        public UpProjectCommand(params string[] projectFileNames) : base(projectFileNames)
        {
        }

        public bool IsDetached { get; internal set; } = true;

        protected override IReadOnlyCollection<string> PrepareArguments()
        {
            var arguments = new List<string> { "up" };
            arguments.AddConditionally(IsDetached, "--detach");
            return arguments.AsReadOnly();
        }
    }
}
