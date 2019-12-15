#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;

#endregion


namespace Eshva.DockerCompose.Commands
{
    public abstract class ServicesCommandBase : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        protected ServicesCommandBase(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        protected ServicesCommandBase(params string[] files) : base(files)
        {
        }

        internal bool DoForAllServices { get; set; }

        internal List<string> Services { get; } = new List<string>();

        protected sealed override string[] PrepareArguments()
        {
            var arguments = new List<string>(PrepareOptions());

            arguments.AddConditionally(
                !string.IsNullOrWhiteSpace(string.Join(" ", Services)),
                string.Join(" ", Services));

            return arguments.ToArray();
        }

        protected abstract string[] PrepareOptions();
    }
}
