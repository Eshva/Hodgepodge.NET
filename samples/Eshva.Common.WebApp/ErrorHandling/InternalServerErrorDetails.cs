#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

#endregion


namespace Eshva.Common.WebApp.ErrorHandling
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public sealed class InternalServerErrorDetails
    {
        public InternalServerErrorDetails(string traceId, IEnumerable<string> errors)
        {
            TraceId = traceId;
            Errors = errors.ToArray();
        }

        [JsonPropertyName("type")]
        public string Type { get; } = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

        [JsonPropertyName("title")]
        public string Title { get; } = "Внутренняя ошибка сервера.";

        [JsonPropertyName("status")]
        public int Status { get; } = (int)HttpStatusCode.InternalServerError;

        [JsonPropertyName("traceId")]
        public string TraceId { get; }

        [JsonPropertyName("errors")]
        public string[] Errors { get; }
    }
}
