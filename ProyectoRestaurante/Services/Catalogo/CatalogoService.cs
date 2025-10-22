using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Infrastructure.Data;
using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.Catalogo
{
    public class CatalogoService : ICatalogoService
    {
        private readonly IDbContextFactory<AppDbContext> factory;
        public CatalogoService(IDbContextFactory<AppDbContext> factory) => this.factory = factory;

        public async Task<List<CategoriaDTO>> ListarCategoriasAsync(bool soloActivas = true)
        {
            using var db = await factory.CreateDbContextAsync();
            var q = db.Categorias.AsQueryable();
            if (soloActivas) q = q.Where(c => c.Activa);
            return await q.OrderBy(c => c.Nombre)
                .Select(c => new CategoriaDTO { Id = c.Id, Nombre = c.Nombre })
                .ToListAsync();
        }

        public async Task<List<ProductoDTO>> ListarProductosAsync(string? filtro = null, int? categoriaId = null, bool soloActivos = true)
        {
            using var db = await factory.CreateDbContextAsync();
            var q = db.Productos.Include(p => p.Categoria).AsQueryable();
            if (soloActivos) q = q.Where(p => p.Activo);
            if (!string.IsNullOrWhiteSpace(filtro)) q = q.Where(p => p.Nombre.Contains(filtro));
            if (categoriaId.HasValue) q = q.Where(p => p.CategoriaId == categoriaId.Value);

            return await q.OrderBy(p => p.Nombre)
                .Select(p => new ProductoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    ImagenArchivo = p.ImagenArchivo,
                    Activo = p.Activo,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria.Nombre
                })
                .ToListAsync();
        }

        public async Task<ProductoDTO?> ObtenerProductoAsync(int id)
        {
            using var db = await factory.CreateDbContextAsync();
            return await db.Productos.Include(p => p.Categoria)
                .Where(p => p.Id == id)
                .Select(p => new ProductoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    ImagenArchivo = p.ImagenArchivo,
                    Activo = p.Activo,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria.Nombre
                }).FirstOrDefaultAsync();
        }
    }
}
