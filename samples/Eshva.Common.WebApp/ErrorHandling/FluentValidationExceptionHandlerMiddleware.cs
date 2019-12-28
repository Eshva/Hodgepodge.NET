#region Usings

using System;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

#endregion


namespace Eshva.Common.WebApp.ErrorHandling
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class FluentValidationExceptionHandlerMiddleware : ExceptionHandlerMiddlewareBase
    {
        public FluentValidationExceptionHandlerMiddleware(RequestDelegate next) : base(next)
        {
        }

        protected override async Task<bool> HandleException(HttpContext context, Exception exception)
        {
            if (!(exception is ValidationException validationException))
            {
                return false;
            }

            await CreateBadRequestResponse(context, validationException);
            return true;
        }

        private Task CreateBadRequestResponse(HttpContext context, ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    new BadRequestProblemDetails(context.Request.HttpContext.TraceIdentifier, validationException.Errors)));
        }
    }
}
