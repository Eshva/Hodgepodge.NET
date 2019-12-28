#region Usings

using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

#endregion


namespace Eshva.Common.WebApp.ErrorHandling
{
    public abstract class ExceptionHandlerMiddlewareBase
    {
        protected ExceptionHandlerMiddlewareBase(RequestDelegate next)
        {
            _next = next;
        }

        [UsedImplicitly]
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (!await HandleException(context, exception))
                {
                    throw;
                }
            }
        }

        protected abstract Task<bool> HandleException(HttpContext context, Exception exception);

        private readonly RequestDelegate _next;
    }
}
