#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Infrastructure;

#endregion


namespace Eshva.DockerCompose.Commands.DownProject
{
    public class DownProjectCommand : CommandBase
    {
        public DownProjectCommand(IProcessStarter processStarter, params string[] projectFileNames)
            : base(processStarter, projectFileNames)
        {
        }

        public DownProjectCommand(params string[] projectFileNames) : base(projectFileNames)
        {
        }

        protected override IReadOnlyCollection<string> PrepareArguments()
        {
            var arguments = new List<string> { "down" };
            return arguments.AsReadOnly();
        }
    }
}
