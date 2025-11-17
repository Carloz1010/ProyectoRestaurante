using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.Reporte
{
    public interface IReporteService
    {
        Task<ReportePedidosDiaDto> ReportePedidosPorDia(DateTime fecha);

        Task<List<ItemMasVendidoDto>> ReporteItemsMasVendidos(DateTime inicio, DateTime fin);

        Task<ReporteVentasDto> ReporteVentas(DateTime inicio, DateTime fin);
    }
}
