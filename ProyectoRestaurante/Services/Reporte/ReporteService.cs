using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Infrastructure.Data;
using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.Reporte
{
    public class ReporteService : IReporteService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public ReporteService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<ReportePedidosDiaDto> ReportePedidosPorDia(DateTime fecha)
        {
            using var db = _dbFactory.CreateDbContext();

            var pedidos = await db.Ordenes
                .Where(o => o.Fecha.Date == fecha.Date)
                .ToListAsync();

            return new ReportePedidosDiaDto
            {
                Fecha = fecha.Date,
                TotalPedidos = pedidos.Count,
                TotalVenta = pedidos.Sum(o => (decimal)o.Total)
            };
        }

        public async Task<List<ItemMasVendidoDto>> ReporteItemsMasVendidos(DateTime inicio, DateTime fin)
        {
            using var db = _dbFactory.CreateDbContext();

            var resultado = await db.OrdenItems
                .Where(i => i.Orden.Fecha.Date >= inicio.Date &&
                            i.Orden.Fecha.Date <= fin.Date)
                .GroupBy(i => new { i.ProductoId, i.Producto.Nombre })
                .Select(g => new ItemMasVendidoDto
                {
                    ProductoId = g.Key.ProductoId,
                    ProductoNombre = g.Key.Nombre,
                    CantidadVendida = g.Sum(i => i.Cantidad)
                })
                .OrderByDescending(x => x.CantidadVendida)
                .ToListAsync();

            return resultado;
        }

        public async Task<ReporteVentasDto> ReporteVentas(DateTime inicio, DateTime fin)
        {
            using var db = _dbFactory.CreateDbContext();

            var ordenes = await db.Ordenes
                .Where(o => o.Fecha.Date >= inicio.Date &&
                            o.Fecha.Date <= fin.Date)
                .ToListAsync();

            return new ReporteVentasDto
            {
                FechaInicio = inicio,
                FechaFin = fin,
                NumeroPedidos = ordenes.Count,
                TotalVentas = ordenes.Sum(o => (decimal)o.Total)
            };
        }
    }
}
