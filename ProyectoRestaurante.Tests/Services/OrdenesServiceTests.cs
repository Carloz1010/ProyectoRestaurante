using ProyectoRestaurante.Domain.Enums;
using ProyectoRestaurante.Services.DTOs;
using ProyectoRestaurante.Services.Ordenes;
using ProyectoRestaurante.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoRestaurante.Tests.Services;

public class OrderServiceTests
{
    [Fact]
    public async Task CrearOrden_DeberiaPersistirConItems_TotalesYEstado()
    {
        await using var factory = new TestSqliteFactory();

        // 1) Creamos un contexto y sembramos datos base
        await using (var dbSeed = await factory.CreateDbContextAsync())
        {
            await SeedHelper.SeedBasicoAsync(dbSeed);
        }

        // 2) Instanciamos OrderService con la fábrica
        IOrdenesService orderService = new OrdenesService(factory);

        // 3) Obtenemos un producto real de la BD para usar su Id y precio
        int productoId;
        int precioProducto;
        await using (var dbLookup = await factory.CreateDbContextAsync())
        {
            var prod = await dbLookup.Productos.AsNoTracking().FirstAsync();
            productoId = prod.Id;
            precioProducto = prod.Precio;
        }

        var dto = new CrearOrdenDTO
        {
            ClienteNombre = "Juan Pérez",
            Telefono = "3001234567",
            TasaImpuesto = 0.19m,
            Items = new List<CrearOrdenItemDTO>
            {
                new() { ProductoId = productoId, Cantidad = 2, PrecioUnitario = precioProducto }
            }
        };

        // 4) Act: crear la orden
        var ordenId = await orderService.CrearOrdenAsync(dto);

        // 5) Assert: leemos con un nuevo contexto y verificamos persistencia
        await using var db = await factory.CreateDbContextAsync();
        var orden = await db.Ordenes
            .Include(o => o.Items)
            .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(o => o.Id == ordenId);

        Assert.NotNull(orden);
        Assert.Equal("Juan Pérez", orden!.ClienteNombre);
        Assert.Equal(EstadoOrden.Recibida, orden.Estado);
        Assert.Single(orden.Items);

        // Cálculos
        var esperadoSubtotal = precioProducto * 2;              // 2 unidades
        var esperadoIva = System.Math.Round(esperadoSubtotal * 0.19m, 2);
        var esperadoTotal = esperadoSubtotal + esperadoIva;

        Assert.Equal(esperadoSubtotal, orden.Subtotal);
        Assert.Equal(esperadoIva, orden.Impuestos);
        Assert.Equal(esperadoTotal, orden.Total);
        Assert.Equal(productoId, orden.Items.First().ProductoId);
        Assert.Equal(precioProducto, orden.Items.First().PrecioUnitario);
        Assert.Equal(2, orden.Items.First().Cantidad);
    }

    [Fact]
    public async Task ActualizarEstado_DeberiaCambiarElValorPersistido()
    {
        await using var factory = new TestSqliteFactory();

        // Seed mínimo + crear orden
        int ordenId;
        {
            await using var dbSeed = await factory.CreateDbContextAsync();
            await SeedHelper.SeedBasicoAsync(dbSeed);
        }

        var service = new OrdenesService(factory);

        {
            // Creamos una orden rápida
            await using var dbLookup = await factory.CreateDbContextAsync();
            var prod = await dbLookup.Productos.AsNoTracking().FirstAsync();

            ordenId = await service.CrearOrdenAsync(new CrearOrdenDTO
            {
                ClienteNombre = "Cliente Test",
                Telefono = "3001234567",
                TasaImpuesto = 0.19m,
                Items = new List<CrearOrdenItemDTO>
                {
                    new() { ProductoId = prod.Id, Cantidad = 1, PrecioUnitario = prod.Precio }
                }
            });
        }

        // Act: actualizar estado
        await service.ActualizarEstadoAsync(ordenId, EstadoOrden.Preparando, "Pasó a cocina");

        // Assert
        await using var db = await factory.CreateDbContextAsync();
        var orden = await db.Ordenes.FirstAsync(o => o.Id == ordenId);
        Assert.Equal(EstadoOrden.Preparando, orden.Estado);
        Assert.Contains("Pasó a cocina", orden.NotasInternas ?? "");
    }

    [Fact]
    public async Task CrearOrdenConMultiplesProductos_DeberiaCalcularTotalesCorrectos()
    {
        await using var factory = new TestSqliteFactory();

        // Seed
        await using (var db = await factory.CreateDbContextAsync())
        {
            await SeedHelper.SeedBasicoAsync(db);
        }

        var service = new OrdenesService(factory);

        // Obtener productos
        int id1, id2;
        int precio1, precio2;
        await using (var db2 = await factory.CreateDbContextAsync())
        {
            var productos = await db2.Productos.AsNoTracking().OrderBy(p => p.Id).ToListAsync();
            id1 = productos[0].Id;
            id2 = productos[1].Id;
            precio1 = productos[0].Precio; // 8000
            precio2 = productos[1].Precio; // 15000
        }

        var dto = new CrearOrdenDTO
        {
            ClienteNombre = "Laura Gómez",
            Telefono = "3119998888",
            TasaImpuesto = 0.19m,
            Items = new List<CrearOrdenItemDTO>
        {
            new() { ProductoId = id1, Cantidad = 2, PrecioUnitario = precio1 }, // 2x8000=16000
            new() { ProductoId = id2, Cantidad = 1, PrecioUnitario = precio2 }  // 1x15000=15000
        }
        };

        // Act
        var ordenId = await service.CrearOrdenAsync(dto);

        // Assert
        await using var db3 = await factory.CreateDbContextAsync();
        var orden = await db3.Ordenes
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == ordenId);

        Assert.NotNull(orden);
        Assert.Equal(2, orden!.Items.Count);
        Assert.Equal(EstadoOrden.Recibida, orden.Estado);

        // Calcular esperado
        var esperadoSubtotal = 16000m + 15000m; // 31000
        var esperadoIva = Math.Round(esperadoSubtotal * 0.19m, 2); // 5890
        var esperadoTotal = esperadoSubtotal + esperadoIva; // 36890

        Assert.Equal(esperadoSubtotal, orden.Subtotal);
        Assert.Equal(esperadoIva, orden.Impuestos);
        Assert.Equal(esperadoTotal, orden.Total);
    }

}
