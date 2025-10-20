namespace ProyectoRestaurante.Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Precio { get; set; }
        public int TiempoPreparacionMin { get; set; }
        public string? ImagenArchivo { get; set; }
        public bool Activo { get; set; } = true;

        // Relación con Categoria
        public Categoria? Categoria { get; set; }
    }
}
