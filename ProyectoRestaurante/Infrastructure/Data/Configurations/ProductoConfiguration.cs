using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoRestaurante.Domain.Entities;

namespace ProyectoRestaurante.Infrastructure.Data.Configurations
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(p => p.Descripcion)
                .HasMaxLength(300);

            builder.Property(p => p.Precio)
                .IsRequired();

            builder.Property(p => p.TiempoPreparacionMin)
                .IsRequired();

            builder.Property(p => p.ImagenArchivo)
                .HasMaxLength(200);

            builder.Property(p => p.Activo)
                .IsRequired();

            // Relación con Categoría
            builder.HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);

            builder.ToTable("Productos");
        }
    }
}
