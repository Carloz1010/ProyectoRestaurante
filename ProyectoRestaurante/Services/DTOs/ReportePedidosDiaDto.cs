namespace ProyectoRestaurante.Services.DTOs
{
    public class ReportePedidosDiaDto
    {
        public DateTime Fecha { get; set; }
        public int TotalPedidos { get; set; }
        public decimal TotalVenta { get; set; }
    }
}
