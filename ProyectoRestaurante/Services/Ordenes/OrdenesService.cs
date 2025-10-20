using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Domain.Entities;
using ProyectoRestaurante.Domain.Enums;
using ProyectoRestaurante.Infrastructure.Data;
using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.Ordenes
{
    public class OrdenesService : IOrdenesService
    {
        private readonly IDbContextFactory<AppDbContext> factory;
        public OrdenesService(IDbContextFactory<AppDbContext> factory) => this.factory = factory;

        public async Task<int> CrearOrdenAsync(CrearOrdenDTO dto)
        {
            using var db = await factory.CreateDbContextAsync();

            var subtotal = dto.Items.Sum(i => i.PrecioUnitario * i.Cantidad);
            var iva = (int)Math.Round(subtotal * dto.TasaImpuesto, MidpointRounding.AwayFromZero);
            var total = subtotal + iva;

            var orden = new Orden
            {
                ClienteNombre = dto.ClienteNombre,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion,
                Fecha = DateTime.UtcNow,
                Estado = EstadoOrden.Recibida,
                Subtotal = subtotal,
                Impuestos = iva,
                Total = total,
                NotasInternas = null
            };

            db.Ordenes.Add(orden);
            await db.SaveChangesAsync();

            var items = dto.Items.Select(i => new OrdenItem
            {
                OrdenId = orden.Id,
                ProductoId = i.ProductoId,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario
            });
            db.OrdenItems.AddRange(items);

            await db.SaveChangesAsync();
            return orden.Id;
        }

        public async Task<OrdenDTO?> ObtenerOrdenAsync(int id)
        {
            using var db = await factory.CreateDbContextAsync();
            return await db.Ordenes
                .Include(o => o.Items).ThenInclude(oi => oi.Producto)
                .Where(o => o.Id == id)
                .Select(o => new OrdenDTO
                {
                    Id = o.Id,
                    ClienteNombre = o.ClienteNombre,
                    Fecha = o.Fecha,
                    Estado = o.Estado,
                    Subtotal = o.Subtotal,
                    Impuestos = o.Impuestos,
                    Total = o.Total,
                    NotasInternas = o.NotasInternas,
                    Items = o.Items.Select(oi => new OrdenItemDTO
                    {
                        ProductoId = oi.ProductoId,
                        Nombre = oi.Producto.Nombre,
                        Cantidad = oi.Cantidad,
                        PrecioUnitario = oi.PrecioUnitario
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<List<OrdenDTO>> BuscarOrdenesAsync(string? cliente = null, EstadoOrden? estado = null, DateTime? desde = null, DateTime? hasta = null)
        {
            using var db = await factory.CreateDbContextAsync();
            var q = db.Ordenes.Include(o => o.Items).ThenInclude(oi => oi.Producto).AsQueryable();

            if (!string.IsNullOrWhiteSpace(cliente)) q = q.Where(o => o.ClienteNombre.Contains(cliente));
            if (estado.HasValue) q = q.Where(o => o.Estado == estado);
            if (desde.HasValue) q = q.Where(o => o.Fecha >= desde.Value);
            if (hasta.HasValue) q = q.Where(o => o.Fecha <= hasta.Value);

            return await q.OrderByDescending(o => o.Fecha)
                .Select(o => new OrdenDTO
                {
                    Id = o.Id,
                    ClienteNombre = o.ClienteNombre,
                    Fecha = o.Fecha,
                    Estado = o.Estado,
                    Subtotal = o.Subtotal,
                    Impuestos = o.Impuestos,
                    Total = o.Total,
                    Items = o.Items.Select(oi => new OrdenItemDTO
                    {
                        ProductoId = oi.ProductoId,
                        Nombre = oi.Producto.Nombre,
                        Cantidad = oi.Cantidad,
                        PrecioUnitario = oi.PrecioUnitario
                    }).ToList()
                }).ToListAsync();
        }

        public async Task ActualizarEstadoAsync(int id, EstadoOrden nuevoEstado, string? notaInterna = null)
        {
            using var db = await factory.CreateDbContextAsync();
            var o = await db.Ordenes.FindAsync(id);
            if (o is null) return;
            o.Estado = nuevoEstado;
            if (!string.IsNullOrWhiteSpace(notaInterna))
                o.NotasInternas = (o.NotasInternas is null ? "" : o.NotasInternas + "\n") + $"[{DateTime.UtcNow}] {notaInterna}";
            await db.SaveChangesAsync();
        }

        public async Task AgregarNotaAsync(int id, string nota)
            => await ActualizarEstadoAsync(id, (await ObtenerEstadoActual(id)), nota);

        private async Task<EstadoOrden> ObtenerEstadoActual(int id)
        {
            using var db = await factory.CreateDbContextAsync();
            var o = await db.Ordenes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return o?.Estado ?? EstadoOrden.Recibida;
        }
    }
}
