using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoRestaurante.Domain.Entities;

namespace ProyectoRestaurante.Infrastructure.Data.Configurations
{
    public class OrdenConfiguration : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.ClienteNombre)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(o => o.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(o => o.Email)
                .HasMaxLength(120);

            builder.Property(o => o.Direccion)
                .HasMaxLength(200);

            // Enums
            builder.Property(o => o.ModalidadPedido)
                .IsRequired();

            builder.Property(o => o.MetodoPago)
                .IsRequired();

            builder.Property(o => o.Estado)
                .IsRequired();

            // Totales
            builder.Property(o => o.Subtotal).IsRequired();
            builder.Property(o => o.Impuestos).IsRequired();
            builder.Property(o => o.Propina).IsRequired();
            builder.Property(o => o.Total).IsRequired();

            builder.Property(o => o.NotasInternas)
                .HasMaxLength(500);

            // Relación 1:N con OrdenItem
            builder.HasMany(o => o.Items)
                .WithOne(i => i.Orden)
                .HasForeignKey(i => i.OrdenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Ordenes");
        }
    }
}
