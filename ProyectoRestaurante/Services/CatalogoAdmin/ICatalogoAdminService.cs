using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.CatalogoAdmin
{
    public interface ICatalogoAdminService
    {
        Task<List<CategoriaDTO>> ObtenerCategoriasAsync();
        Task<CategoriaDTO> CrearCategoriaAsync(CategoriaDTO dto);
        Task ActualizarCategoriaAsync(CategoriaDTO dto);
        Task EliminarCategoriaAsync(int id);

        Task<List<ProductoDTO>> ObtenerProductosAsync();
        Task<ProductoDTO> CrearProductoAsync(ProductoDTO dto);
        Task ActualizarProductoAsync(ProductoDTO dto);
        Task EliminarProductoAsync(int id);
    }
}
