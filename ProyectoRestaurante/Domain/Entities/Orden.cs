using ProyectoRestaurante.Domain.Enums;

namespace ProyectoRestaurante.Domain.Entities
{
    public class Orden
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        // Datos del cliente
        public string ClienteNombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Direccion { get; set; }

        // Modalidad del pedido
        public ModalidadPedido ModalidadPedido { get; set; }
        public string? UbicacionMesa { get; set; }

        // Pago
        public MetodoPago MetodoPago { get; set; }
        public bool PagoConfirmado { get; set; } = false;

        // Totales
        public int Subtotal { get; set; }
        public int Impuestos { get; set; }
        public int Propina { get; set; }
        public int Total { get; set; }

        // Estado
        public EstadoOrden Estado { get; set; } = EstadoOrden.Recibida;

        // Notas internas
        public string? NotasInternas { get; set; }

        // Relación con OrdenItem
        public List<OrdenItem> Items { get; set; } = new();
    }
}
