using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using ProyectoRestaurante.Components;
using ProyectoRestaurante.Infrastructure.Data;
using ProyectoRestaurante.Services.Autenticacion;
using ProyectoRestaurante.Services.Carrito;
using ProyectoRestaurante.Services.Catalogo;
using ProyectoRestaurante.Services.CatalogoAdmin;
using ProyectoRestaurante.Services.Ordenes;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<IOrdenesService, OrdenesService>();
builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
builder.Services.AddScoped<ICatalogoAdminService, CatalogoAdminService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AutenticacionStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<AutenticacionStateProvider>());

//rutas de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/admin/login";
    options.LogoutPath = "/admin/logout";
    options.AccessDeniedPath = "/admin/acceso-denegado";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("role", "Admin"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

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
