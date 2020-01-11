#region Usings

using System;

#endregion


namespace Eshva.Common.WebApp.ApplicationConfiguration
{
    public sealed class ApplicationConfigurationException : Exception
    {
        public ApplicationConfigurationException(string message) : base(message)
        {
        }
    }
}
