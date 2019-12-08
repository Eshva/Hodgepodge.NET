#region Usings

using System;

#endregion


namespace Eshva.DockerCompose.Exceptions
{
    public abstract class DockerComposeException : Exception
    {
        protected DockerComposeException(string message) : base(message)
        {
        }

        protected DockerComposeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
