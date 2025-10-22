using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Components;
using ProyectoRestaurante.Infrastructure.Data;
using ProyectoRestaurante.Services.Catalogo;
using ProyectoRestaurante.Services.Ordenes;
using ProyectoRestaurante.Services.Carrito;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);

});

builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<IOrdenesService, OrdenesService>();
builder.Services.AddScoped<ICarritoService, CarritoService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var dbContext = dbContextFactory.CreateDbContext();

    var seeder = new DataSeeder(dbContext);
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
