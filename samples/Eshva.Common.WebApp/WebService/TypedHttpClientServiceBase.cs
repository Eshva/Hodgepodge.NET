#region Usings

using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

#endregion


namespace Eshva.Common.WebApp.WebService
{
    public abstract class TypedHttpClientServiceBase
    {
        protected TypedHttpClientServiceBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpClient HttpClient { get; }

        protected StringContent MakeJsonContent<TPayload>(TPayload payload) =>
            new StringContent(
                JsonSerializer.Serialize(payload, SerializerOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

        private JsonSerializerOptions SerializerOptions { get; } =
            new JsonSerializerOptions
            {
                AllowTrailingCommas = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
    }
}
