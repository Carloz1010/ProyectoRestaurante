namespace ProyectoRestaurante.Services.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activa { get; set; } = true;
        public int? Orden { get; set; }
    }
}
