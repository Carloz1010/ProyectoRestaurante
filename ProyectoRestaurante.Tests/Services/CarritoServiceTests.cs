using Microsoft.AspNetCore.Cors.Infrastructure;
using ProyectoRestaurante.Services.Carrito;
using ProyectoRestaurante.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProyectoRestaurante.Tests.Services;

public class CartServiceTests
{
    [Fact]
    public void Agregar_Actualizar_Quitar_Y_Totales_DeberianSerCorrectos()
    {
        // Arrange
        var cart = new CarritoService();
        var p1 = new ProductoDTO { Id = 1, Nombre = "Limonada", Precio = 8000 };
        var p2 = new ProductoDTO { Id = 2, Nombre = "Hamburguesa", Precio = 15000 };

        // Act
        cart.Agregar(p1, 2); // 2 x 8000 = 16000
        cart.Agregar(p2, 1); // 1 x 15000 = 15000

        // Assert intermedio
        Assert.Equal(2, cart.Items.Count);
        Assert.Equal(31000m, cart.Subtotal());
        Assert.Equal(5890m, cart.Impuestos(0.19m));
        Assert.Equal(36890m, cart.Total(0.19m));

        // Act: actualizar cantidad
        cart.ActualizarCantidad(1, 3); // ahora 3 x 8000 = 24000 + 15000 = 39000
        Assert.Equal(39000m, cart.Subtotal());

        // Act: quitar
        cart.Quitar(2); // quita hamburguesa, queda solo limonada (3)
        Assert.Single(cart.Items);
        Assert.Equal(24000m, cart.Subtotal());

        // Act: vaciar
        cart.Vaciar();
        Assert.Empty(cart.Items);
        Assert.Equal(0m, cart.Subtotal());
    }

    [Fact]
    public void NoDebePermitirCantidadNegativa_YDebeManejarCarritoVacioCorrectamente()
    {
        // Arrange
        var cart = new CarritoService();
        var p1 = new ProductoDTO { Id = 1, Nombre = "Pizza", Precio = 12000 };

        // Act 1: intentar agregar con cantidad negativa
        cart.Agregar(p1, -3); // debería ignorar o no agregar
        Assert.Empty(cart.Items); // no debe agregar nada

        // Act 2: agregar normalmente
        cart.Agregar(p1, 2);
        Assert.Single(cart.Items);
        Assert.Equal(24000m, cart.Subtotal());

        // Act 3: actualizar con cantidad 0 (debe quitarlo)
        cart.ActualizarCantidad(1, 0);
        Assert.Empty(cart.Items);

        // Act 4: carrito vacío, totales deben ser cero
        Assert.Equal(0m, cart.Subtotal());
        Assert.Equal(0m, cart.Impuestos(0.19m));
        Assert.Equal(0m, cart.Total(0.19m));
    }
}
