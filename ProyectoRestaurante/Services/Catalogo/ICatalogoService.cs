using ProyectoRestaurante.Services.DTOs;

namespace ProyectoRestaurante.Services.Catalogo
{
    public interface ICatalogoService
    {
        Task<List<CategoriaDTO>> ListarCategoriasAsync(bool soloActivas = true);
        Task<List<ProductoDTO>> ListarProductosAsync(string? filtro = null, int? categoriaId = null, bool soloActivos = true);
        Task<ProductoDTO?> ObtenerProductoAsync(int id);
    }
}
