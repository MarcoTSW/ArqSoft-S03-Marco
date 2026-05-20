using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Ruta del archivo JSON
var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "data", "items.json");

// Registrar repositorio y servicios
builder.Services.AddSingleton<IItemRepository>(new JsonItemRepository(jsonPath));
builder.Services.AddScoped<ItemService>();
builder.Services.AddAuthorization();

var app = builder.Build();

// 2. Configurar el pipeline de solicitudes HTTP (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Optimización de archivos estáticos para .NET 9
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
).WithStaticAssets(); // <-- Aquí estaba el error de compilación (faltaba cerrar el paréntesis)

app.Run();