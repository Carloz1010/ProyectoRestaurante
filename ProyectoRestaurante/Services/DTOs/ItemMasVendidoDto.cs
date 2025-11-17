namespace ProyectoRestaurante.Services.DTOs
{
    public class ItemMasVendidoDto
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
    }
}
