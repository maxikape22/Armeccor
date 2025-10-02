using DTO.ObjetosDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armeccor.Client.Shared
{
    public interface INotificationService
    {
        Task<List<NotificacionDTO>> GetAllAsync();
        Task LimpiarTodas();
        Task MarcarTodasComoLeidas();
    }
}
