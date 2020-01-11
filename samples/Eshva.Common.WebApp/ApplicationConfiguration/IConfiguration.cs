#region Usings

using Microsoft.Extensions.Configuration;

#endregion


namespace Eshva.Common.WebApp.ApplicationConfiguration
{
    public interface IConfiguration
    {
        void LoadFrom(IConfigurationSection configurationSection);

        void Validate();
    }
}
