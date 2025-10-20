using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Domain.Entities;

namespace ProyectoRestaurante.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Si ya hay categorías, no volver a sembrar
            if (await _context.Categorias.AnyAsync()) return;

            // --- Categorías ---
            var categorias = new List<Categoria>
            {
                new() { Nombre = "Hamburguesas", Activa = true, Orden = 1 },
                new() { Nombre = "Bebidas", Activa = true, Orden = 2 },
                new() { Nombre = "Postres", Activa = true, Orden = 3 },
            };
            await _context.Categorias.AddRangeAsync(categorias);
            await _context.SaveChangesAsync();

            // --- Productos ---
            var productos = new List<Producto>
            {
                new() { Nombre = "Hamburguesa Clásica", CategoriaId = categorias[0].Id, Precio = 18000, TiempoPreparacionMin = 10, ImagenArchivo = "hamburguesa1.jpg", Activo = true },
                new() { Nombre = "Hamburguesa Doble", CategoriaId = categorias[0].Id, Precio = 22000, TiempoPreparacionMin = 12, ImagenArchivo = "hamburguesa2.jpg", Activo = true },
                new() { Nombre = "Gaseosa 400ml", CategoriaId = categorias[1].Id, Precio = 3000, TiempoPreparacionMin = 0, ImagenArchivo = "bebida1.jpg", Activo = true },
                new() { Nombre = "Malteada", CategoriaId = categorias[1].Id, Precio = 7000, TiempoPreparacionMin = 0, ImagenArchivo = "bebida2.jpg", Activo = true },
                new() { Nombre = "Brownie con helado", CategoriaId = categorias[2].Id, Precio = 12000, TiempoPreparacionMin = 5, ImagenArchivo = "postre1.jpg", Activo = true }
            };

            await _context.Productos.AddRangeAsync(productos);
            await _context.SaveChangesAsync();
        }
    }
}
