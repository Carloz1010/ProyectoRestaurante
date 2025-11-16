namespace ProyectoRestaurante.Domain.Entities
{
    public class UsuarioAdministrador
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}
