using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Domain.Entities;
using ProyectoRestaurante.Infrastructure.Data;
using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.CatalogoAdmin
{
    public class CatalogoAdminService : ICatalogoAdminService
    {
        private readonly IDbContextFactory<AppDbContext> factory;

        public CatalogoAdminService(IDbContextFactory<AppDbContext> factory)
        {
            this.factory = factory;
        }

        // --------- Categorías ---------
        public async Task<List<CategoriaDTO>> ObtenerCategoriasAsync()
        {
            using var db = await factory.CreateDbContextAsync();

            return await db.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Orden ?? int.MaxValue)
                .ThenBy(c => c.Nombre)
                .Select(c => new CategoriaDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Activa = c.Activa,
                    Orden = c.Orden
                })
                .ToListAsync();
        }

        public async Task<CategoriaDTO> CrearCategoriaAsync(CategoriaDTO dto)
        {
            using var db = await factory.CreateDbContextAsync();

            var cat = new Categoria
            {
                Nombre = dto.Nombre,
                Activa = dto.Activa,
                Orden = dto.Orden
            };

            db.Categorias.Add(cat);
            await db.SaveChangesAsync();

            dto.Id = cat.Id;
            return dto;
        }

        public async Task ActualizarCategoriaAsync(CategoriaDTO dto)
        {
            using var db = await factory.CreateDbContextAsync();

            var cat = await db.Categorias.FindAsync(dto.Id);
            if (cat is null)
                throw new InvalidOperationException("Categoría no encontrada.");

            cat.Nombre = dto.Nombre;
            cat.Activa = dto.Activa;
            cat.Orden = dto.Orden;

            await db.SaveChangesAsync();
        }

        public async Task EliminarCategoriaAsync(int id)
        {
            using var db = await factory.CreateDbContextAsync();

            bool tieneProductos = await db.Productos.AnyAsync(p => p.CategoriaId == id);
            if (tieneProductos)
                throw new InvalidOperationException("No se puede eliminar una categoría con productos asociados.");

            var cat = await db.Categorias.FindAsync(id);
            if (cat is null)
                return;

            db.Categorias.Remove(cat);
            await db.SaveChangesAsync();
        }

        // --------- Productos ---------
        public async Task<List<ProductoDTO>> ObtenerProductosAsync()
        {
            using var db = await factory.CreateDbContextAsync();

            return await db.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .OrderBy(p => p.Categoria!.Orden ?? int.MaxValue)
                .ThenBy(p => p.Categoria!.Nombre)
                .ThenBy(p => p.Nombre)
                .Select(p => new ProductoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion ?? string.Empty,
                    Precio = p.Precio,
                    ImagenArchivo = p.ImagenArchivo,
                    Activo = p.Activo,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : string.Empty,
                    TiempoPreparacionMin = p.TiempoPreparacionMin
                })
                .ToListAsync();
        }

        public async Task<ProductoDTO> CrearProductoAsync(ProductoDTO dto)
        {
            using var db = await factory.CreateDbContextAsync();

            var categoria = await db.Categorias.FirstOrDefaultAsync(c => c.Id == dto.CategoriaId);
            if (categoria is null)
                throw new InvalidOperationException("La categoría seleccionada no existe.");

            var prod = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                ImagenArchivo = dto.ImagenArchivo,
                Activo = dto.Activo,
                CategoriaId = dto.CategoriaId,
                TiempoPreparacionMin = dto.TiempoPreparacionMin
            };

            db.Productos.Add(prod);
            await db.SaveChangesAsync();

            dto.Id = prod.Id;
            dto.CategoriaNombre = categoria.Nombre;
            return dto;
        }

        public async Task ActualizarProductoAsync(ProductoDTO dto)
        {
            using var db = await factory.CreateDbContextAsync();

            var prod = await db.Productos.FindAsync(dto.Id);
            if (prod is null)
                throw new InvalidOperationException("Producto no encontrado.");

            var categoria = await db.Categorias.FirstOrDefaultAsync(c => c.Id == dto.CategoriaId);
            if (categoria is null)
                throw new InvalidOperationException("La categoría seleccionada no existe.");

            prod.Nombre = dto.Nombre;
            prod.Descripcion = dto.Descripcion;
            prod.Precio = dto.Precio;
            prod.ImagenArchivo = dto.ImagenArchivo;
            prod.Activo = dto.Activo;
            prod.CategoriaId = dto.CategoriaId;
            prod.TiempoPreparacionMin = dto.TiempoPreparacionMin;

            await db.SaveChangesAsync();
        }

        public async Task EliminarProductoAsync(int id)
        {
            using var db = await factory.CreateDbContextAsync();

            var prod = await db.Productos.FindAsync(id);
            if (prod is null)
                return;

            db.Productos.Remove(prod);
            await db.SaveChangesAsync();
        }
    }
}
