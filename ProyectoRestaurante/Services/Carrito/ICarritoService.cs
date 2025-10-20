using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.Carrito
{
    public interface ICarritoService
    {
        IReadOnlyList<CarritoItemDTO> Items { get; }
        int Subtotal();
        int Impuestos(decimal tasaIva);
        int Total(decimal tasaIva);

        void Agregar(ProductoDTO p, int cantidad = 1);
        void ActualizarCantidad(int productoId, int nuevaCantidad);
        void Quitar(int productoId);
        void Vaciar();
    }
}
