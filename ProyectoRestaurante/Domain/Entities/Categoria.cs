namespace ProyectoRestaurante.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activa { get; set; } = true;
        public int? Orden { get; set; }

        // Relación 1:N con Producto
        public List<Producto> Productos { get; set; } = new();
    }
}
