using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoRestaurante.Domain.Entities;

namespace ProyectoRestaurante.Infrastructure.Data.Configurations
{
    public class OrdenItemConfiguration : IEntityTypeConfiguration<OrdenItem>
    {
        public void Configure(EntityTypeBuilder<OrdenItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.NombreProducto)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(i => i.PrecioUnitario)
                .IsRequired();

            builder.Property(i => i.Cantidad)
                .IsRequired();

            builder.Property(i => i.TotalLinea)
                .IsRequired();

            builder.ToTable("OrdenItems");
        }
    }
}
