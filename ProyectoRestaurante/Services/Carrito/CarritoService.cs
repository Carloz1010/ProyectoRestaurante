using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.Carrito
{
    public class CarritoService  : ICarritoService
    {
        private readonly List<CarritoItemDTO> items = new();
        public IReadOnlyList<CarritoItemDTO> Items => items;

        public void Agregar(ProductoDTO p, int cantidad = 1)
        {
            if (cantidad <= 0) return;
            var i = items.FirstOrDefault(x => x.ProductoId == p.Id);
            if (i is null)
                items.Add(new CarritoItemDTO { ProductoId = p.Id, Nombre = p.Nombre, PrecioUnitario = p.Precio, Cantidad = cantidad });
            else
                i.Cantidad += cantidad;
        }

        public void ActualizarCantidad(int productoId, int nuevaCantidad)
        {
            var i = items.FirstOrDefault(x => x.ProductoId == productoId);
            if (i is null) return;
            if (nuevaCantidad <= 0) items.Remove(i);
            else i.Cantidad = nuevaCantidad;
        }

        public void Quitar(int productoId) => items.RemoveAll(x => x.ProductoId == productoId);
        public void Vaciar() => items.Clear();

        public int Subtotal() => items.Sum(x => x.Subtotal);
        public int Impuestos(decimal tasaIva) => (int)Math.Round(Subtotal() * tasaIva, MidpointRounding.AwayFromZero);
        public int Total(decimal tasaIva) => Subtotal() + Impuestos(tasaIva);
    }
}
