using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoRestaurante.Domain.Entities;

namespace ProyectoRestaurante.Infrastructure.Data.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(c => c.Activa)
                .IsRequired();

            builder.Property(c => c.Orden)
                .IsRequired(false);

            // Relación 1:N con Productos
            builder.HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Categorias");
        }
    }
}
