namespace ProyectoRestaurante.Services.DTOs
{
    public class CarritoItemDTO
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int Subtotal => PrecioUnitario * Cantidad;
    }
}