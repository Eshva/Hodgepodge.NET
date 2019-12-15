namespace Eshva.DockerCompose.Commands.StartServices
{
    /// <summary>
    /// The builder for <see cref="StartServicesCommand"/> command.
    /// </summary>
    public sealed class StartServicesCommandBuilder : CommandBuilderBase<StartServicesCommand>
    {
        internal StartServicesCommandBuilder(StartServicesCommand command) : base(command)
        {
        }

        /// <summary>
        /// Start existing containers for all services in project files.
        /// </summary>
        /// <returns>
        /// Same command builder.
        /// </returns>
        public StartServicesCommandBuilder StartAllServices()
        {
            Command.DoStartAllServices = true;
            return this;
        }

        /// <summary>
        /// Start existing containers for services specified in <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services to start.
        /// </param>
        /// <returns>
        /// Same builder.
        /// </returns>
        public StartServicesCommandBuilder StartServices(params string[] services)
        {
            Command.Services.AddRange(services);
            return this;
        }
    }
}
