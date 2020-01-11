#region Usings

using System;

#endregion


namespace Eshva.Common.WebApp.WebService
{
    public sealed class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message) : base(message)
        {
        }
    }
}
