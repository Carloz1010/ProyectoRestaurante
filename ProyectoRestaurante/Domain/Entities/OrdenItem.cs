namespace ProyectoRestaurante.Domain.Entities
{
    public class OrdenItem
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }

        public string NombreProducto { get; set; } = string.Empty;
        public int PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int TotalLinea { get; set; }

        // Relación con Orden
        public Orden? Orden { get; set; }
        public Producto Producto { get; set; } = null!;
    }
}
