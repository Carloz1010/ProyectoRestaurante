using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using ProyectoRestaurante.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoRestaurante.Tests.TestHelpers;

/// <summary>
/// Fábrica de AppDbContext para pruebas de integración con una
/// conexión SQLite en memoria compartida (mientras el objeto viva).
/// </summary>
public sealed class TestSqliteFactory : IDbContextFactory<AppDbContext>, IAsyncDisposable, IDisposable
{
    private readonly SqliteConnection connection;
    private readonly DbContextOptions<AppDbContext> options;
    private bool initialized;

    public TestSqliteFactory()
    {
        // "Mode=Memory;Cache=Shared" permite múltiples contextos usando la misma conexión si se abre.
        connection = new SqliteConnection("DataSource=:memory:;Mode=Memory;Cache=Shared");
        connection.Open();

        options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .UseLoggerFactory(new NullLoggerFactory()) // silenciar logs en test
            .Options;
    }

    /// <summary>
    /// Crea un contexto conectado a la misma BD en memoria.
    /// La primera vez, asegura crear el esquema.
    /// </summary>
    public AppDbContext CreateDbContext()
    {
        var ctx = new AppDbContext(options);
        EnsureCreatedOnceAsync(ctx).GetAwaiter().GetResult();
        return ctx;
    }

    public async Task<AppDbContext> CreateDbContextAsync()
    {
        var ctx = new AppDbContext(options);
        await EnsureCreatedOnceAsync(ctx);
        return ctx;
    }

    private async Task EnsureCreatedOnceAsync(AppDbContext ctx)
    {
        if (initialized) return;

        // Si usas Migraciones, puedes usar: await ctx.Database.MigrateAsync();
        // Para rapidez en test: EnsureCreated crea el esquema según tu modelo.
        await ctx.Database.EnsureCreatedAsync();
        initialized = true;
    }

    public void Dispose()
    {
        connection.Close();
        connection.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }
}