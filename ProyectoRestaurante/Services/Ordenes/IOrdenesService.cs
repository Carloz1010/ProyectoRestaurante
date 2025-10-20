using ProyectoRestaurante.Domain.Enums;
using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.Ordenes
{
    public interface IOrdenesService
    {
        Task<int> CrearOrdenAsync(CrearOrdenDTO dto);
        Task<OrdenDTO?> ObtenerOrdenAsync(int id);
        Task<List<OrdenDTO>> BuscarOrdenesAsync(string? cliente = null, EstadoOrden? estado = null, DateTime? desde = null, DateTime? hasta = null);
        Task ActualizarEstadoAsync(int id, EstadoOrden nuevoEstado, string? notaInterna = null);
        Task AgregarNotaAsync(int id, string nota);
    }
}
