
using System.Text;
using System.Text.Json;

namespace Armeccor.Client.Servicios
{
    public class HttpServicio : IHttpServicio
    {
        private readonly HttpClient http;

        public HttpServicio(HttpClient http)
        {
            this.http = http;
        }
        public async Task<HttpRespuesta<T>> Get<T>(string url)
        {

            var response = await http.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {

                var respuesta = await DeserializarRepuesta<T>(response);
                return new HttpRespuesta<T>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<T>(default, true, response);
            }
        }



        public async Task<HttpRespuesta<object>> Post<T>(string url, T enviar)
        {

            try
            {
                var enviarJson = JsonSerializer.Serialize(enviar);
                var enviarContent = new StringContent(enviarJson,
                                                     Encoding.UTF8,
                                                     "application/json");
                var respuesta = await http.PostAsync(url, enviarContent);
                return new HttpRespuesta<object>(null,
                                                 !respuesta.IsSuccessStatusCode,
                                                 respuesta);

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<HttpRespuesta<object>> Put<T>(string url, T enviar)
        {
            try
            {
                var enviarJson = JsonSerializer.Serialize(enviar);
                var enviarContent = new StringContent(enviarJson,
                                                      Encoding.UTF8,
                                                      "application/json");
                var respuesta = await http.PutAsync(url, enviarContent);
                return new HttpRespuesta<object>(null,
                                                 !respuesta.IsSuccessStatusCode,
                                                 respuesta);
            }
            catch (Exception e) { throw; }
        }

        public async Task<HttpRespuesta<object>> Delete(string url)

        {
            var respuesta = await http.DeleteAsync(url);
            return new HttpRespuesta<object>(null,
                                             !respuesta.IsSuccessStatusCode,
                                             respuesta);

        }


        private async Task<T> DeserializarRepuesta<T>(HttpResponseMessage response)
        {
            var respuestaStr = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaStr,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

    }
}