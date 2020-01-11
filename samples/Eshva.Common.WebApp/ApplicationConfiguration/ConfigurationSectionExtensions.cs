#region Usings

using Microsoft.Extensions.Configuration;

#endregion


namespace Eshva.Common.WebApp.ApplicationConfiguration
{
    public static class ConfigurationSectionExtensions
    {
        /// <summary>
        /// Loads and verifies config of type <typeparamref name="TConfiguration"/> from a configuration section in
        /// <paramref name="configurationSection"/>.
        /// </summary>
        /// <typeparam name="TConfiguration">
        /// Config type.
        /// </typeparam>
        /// <param name="configurationSection">
        /// Configuration section defined in <code>appSettings.json</code> file.
        /// </param>
        /// <returns>
        /// Config object of type <typeparamref name="TConfiguration"/>.
        /// </returns>
        /// <exception cref="ApplicationConfigurationException">
        /// Some errors found in configuration section.
        /// </exception>
        public static TConfiguration LoadConfiguration<TConfiguration>(this IConfigurationSection configurationSection)
            where TConfiguration : IConfiguration, new()
        {
            var configuration = new TConfiguration();
            configuration.LoadFrom(configurationSection);
            configuration.Validate();
            return configuration;
        }
    }
}
