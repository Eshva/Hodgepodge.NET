#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using JetBrains.Annotations;

#endregion


namespace Eshva.Common.WebApp.ErrorHandling
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public sealed class BadRequestProblemDetails
    {
        public BadRequestProblemDetails(string traceId, IEnumerable<ValidationFailure> failures)
        {
            TraceId = traceId;
            Errors = failures
                     .GroupBy(failure => failure.PropertyName)
                     .ToDictionary(
                         group => group.Key,
                         group => group.Select(failure => failure.ErrorMessage).ToArray());
        }

        [JsonPropertyName("type")]
        public string Type { get; } = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        [JsonPropertyName("title")]
        public string Title { get; } = "Получены некорректные данные запроса.";

        [JsonPropertyName("status")]
        public int Status { get; } = (int)HttpStatusCode.BadRequest;

        [JsonPropertyName("traceId")]
        public string TraceId { get; }

        [JsonPropertyName("errors")]
        public IDictionary<string, string[]> Errors { get; }
    }
}
