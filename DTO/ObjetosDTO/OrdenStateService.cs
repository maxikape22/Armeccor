using DTO.ObjetosDTO;
using System.Net.Http.Json;

public class OrdenStateService
{
    private readonly HttpClient _httpClient;
    private List<OrdenDetalleDTO> _ordenes = new();

    // Evento que otros componentes pueden suscribir
    public event Action? OnChange;

    public OrdenStateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<OrdenDetalleDTO> Ordenes => _ordenes;

    public async Task LoadOrders()
    {
        try
        {
            _ordenes = await _httpClient.GetFromJsonAsync<List<OrdenDetalleDTO>>("api/Ordenes") ?? new();
            NotifyStateChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar órdenes en el servicio: {ex.Message}");
            // Manejo de errores, si es necesario
        }
    }

    // Método para notificar a los suscriptores que los datos han cambiado
    public void NotifyStateChanged() => OnChange?.Invoke();

    // Métodos para actualizar datos (opcional, podrías hacerlo en los componentes)
    public async Task UpdateAreaStatus(int areaDetalleId, string nuevoEstado)
    {
        var payload = new { Estado = nuevoEstado };
        var response = await _httpClient.PutAsJsonAsync($"api/Area_Detalle_Orden/{areaDetalleId}/Estado", payload);
        if (response.IsSuccessStatusCode)
        {
            // Opcional: Recargar todas las órdenes para que el estado se actualice
            await LoadOrders();
        }
    }
}