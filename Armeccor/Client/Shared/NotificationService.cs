using DTO.ObjetosDTO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Armeccor.Client.Shared
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _http;

        public NotificationService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<NotificacionDTO>> GetAllAsync()
        {
            var dtos = await _http.GetFromJsonAsync<List<NotificacionDTO>>("api/Notificaciones");

            return dtos.Select(dto => new NotificacionDTO
            {
                Mensaje = dto.Mensaje,
                Tipo = dto.Tipo,
                EsLeida = dto.EsLeida,
                Fecha = dto.Fecha
            }).ToList();
        }

        public async Task MarcarTodasComoLeidas()
        {
            await _http.PutAsync("api/Notificaciones/marcar-todas-leidas", null);
        }

        public async Task LimpiarTodas()
        {
            await _http.DeleteAsync("api/Notificaciones/limpiar");
        }

    }
}

