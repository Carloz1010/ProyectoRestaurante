namespace ProyectoRestaurante.Services.DTOs
{
    public class ReporteVentasDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumeroPedidos { get; set; }
        public decimal TotalVentas { get; set; }
    }
}
