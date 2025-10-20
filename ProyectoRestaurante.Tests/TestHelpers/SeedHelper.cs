using ProyectoRestaurante.Domain.Entities;
using ProyectoRestaurante.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoRestaurante.Tests.TestHelpers;

public static class SeedHelper
{
    public static async Task SeedBasicoAsync(AppDbContext db)
    {
        if (!await db.Categorias.AnyAsync())
        {
            var bebidas = new Categoria { Nombre = "Bebidas", Activa = true };
            var comidas = new Categoria { Nombre = "Comidas", Activa = true };

            db.Categorias.AddRange(bebidas, comidas);

            db.Productos.AddRange(
                new Producto { Nombre = "Limonada", Descripcion = "Refrescante", Precio = 8000, Activo = true, Categoria = bebidas },
                new Producto { Nombre = "Hamburguesa", Descripcion = "Clásica", Precio = 15000, Activo = true, Categoria = comidas }
            );

            await db.SaveChangesAsync();
        }
    }
}
