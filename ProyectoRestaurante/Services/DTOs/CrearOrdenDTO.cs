using ProyectoRestaurante.Domain.Enums;

namespace ProyectoRestaurante.Services.DTOs
{
    public class CrearOrdenDTO
    {
        public string ClienteNombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public List<CrearOrdenItemDTO> Items { get; set; } = new();
        public decimal TasaImpuesto { get; set; } = 0.1m;

        public ModalidadPedido Modalidad { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public string? Mesa { get; set; }
    }
}
