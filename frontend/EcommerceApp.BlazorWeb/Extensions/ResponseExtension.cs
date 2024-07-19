using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace EcommerceApp.BlazorWeb.Extensions
{
    internal static class ResponseExtension
    {
        public static async Task<T> ToClassInstance<T>(this HttpResponseMessage responseMessage)
        {
            var responseString = await responseMessage.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            
            return response ?? default(T)!;
        }
    }
}
