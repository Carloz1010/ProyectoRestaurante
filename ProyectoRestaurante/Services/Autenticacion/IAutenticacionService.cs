using ProyectoRestaurante.Domain.Entities;

namespace ProyectoRestaurante.Services.Autenticacion
{
    public interface IAutenticacionService
    {
        Task<UsuarioAdministrador?> ValidarCredencialesAsync(string userName, string contraseña);
        Task<UsuarioAdministrador> CrearAdminAsync(string userName, string contraseña);
        Task<List<UsuarioAdministrador>> ObtenerAdminAsync();
        Task EliminarAdminAsync(int id);

    }
}
