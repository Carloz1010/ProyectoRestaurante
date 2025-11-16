using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Domain.Entities;

namespace ProyectoRestaurante.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Orden> Ordenes => Set<Orden>();
        public DbSet<OrdenItem> OrdenItems => Set<OrdenItem>();
        public DbSet<UsuarioAdministrador> UsuariosAdministradores => Set<UsuarioAdministrador>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
