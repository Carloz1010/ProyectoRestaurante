namespace ProyectoRestaurante.Services.DTOs
{
    public class OrdenItemDTO
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int PrecioUnitario { get; set; }
        public int Subtotal => PrecioUnitario * Cantidad;
    }
}
