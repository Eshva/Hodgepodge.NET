#region Usings

using System;

#endregion


namespace Eshva.DockerCompose.Exceptions
{
    public sealed class ProcessStartException : DockerComposeException
    {
        public ProcessStartException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
