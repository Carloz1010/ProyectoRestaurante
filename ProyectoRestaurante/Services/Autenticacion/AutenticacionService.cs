using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Domain.Entities;
using ProyectoRestaurante.Infrastructure.Data;

namespace ProyectoRestaurante.Services.Autenticacion
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly PasswordHasher<UsuarioAdministrador> _hasher = new();
        private readonly IDbContextFactory<AppDbContext> factory;
        public AutenticacionService(IDbContextFactory<AppDbContext> factory) => this.factory = factory;

        public async Task<UsuarioAdministrador> CrearAdminAsync(string userName, string contraseña)
        {
            using var db = await factory.CreateDbContextAsync();
            if (await db.UsuariosAdministradores.AnyAsync(u => u.UserName == userName))
                throw new InvalidOperationException("El usuario ya existe");

            var user = new UsuarioAdministrador { UserName = userName };
            user.Contraseña = _hasher.HashPassword(user, contraseña);
            db.UsuariosAdministradores.Add(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<List<UsuarioAdministrador>> ObtenerAdminAsync()
        {
            using var db = await factory.CreateDbContextAsync();
            return await db.UsuariosAdministradores
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        public async Task<UsuarioAdministrador?> ValidarCredencialesAsync(string userName, string contraseña)
        {
            using var db = await factory.CreateDbContextAsync();
            var user = await db.UsuariosAdministradores.FirstOrDefaultAsync(u => u.UserName == userName && u.Activo);
            if (user is null) return null;
            var result = _hasher.VerifyHashedPassword(user, user.Contraseña, contraseña);
            if (result == PasswordVerificationResult.Success)
                return user;
            else
                return null;
        }

        public async Task EliminarAdminAsync(int id)
        {
            using var db = await factory.CreateDbContextAsync();
            var user = await db.UsuariosAdministradores.FindAsync(id);
            if (user is null)
                return;
            db.UsuariosAdministradores.Remove(user);
            await db.SaveChangesAsync();
        }
    }
}
