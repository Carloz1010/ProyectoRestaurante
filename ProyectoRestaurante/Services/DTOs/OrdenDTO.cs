using ProyectoRestaurante.Domain.Enums;

namespace ProyectoRestaurante.Services.DTOs
{
    public class OrdenDTO
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public EstadoOrden Estado { get; set; }
        public int Subtotal { get; set; }
        public int Impuestos { get; set; }
        public int Total { get; set; }
        public List<OrdenItemDTO> Items { get; set; } = new();
        public string? NotasInternas { get; set; }
    }
}
