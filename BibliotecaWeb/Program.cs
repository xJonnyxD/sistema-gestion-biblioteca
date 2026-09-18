using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using BibliotecaWeb.Data;
using BibliotecaWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Entity Framework Core: registra el ApplicationDbContext y lo configura para
// comunicarse con SQL Server usando la cadena de conexión "BibliotecaDB".
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDB")));

// Inyección de Dependencias: se asocia la abstracción IAutorService con su
// implementación AutorService, usando el ciclo de vida Scoped (una instancia por petición).
builder.Services.AddScoped<IAutorService, AutorService>();

// Reto (SOLID / IoC / DI): para usar la segunda implementación basta con cambiar
// la línea anterior por la siguiente. El AutoresController no requiere modificación.
// builder.Services.AddScoped<IAutorService, AutorServiceDemo>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Aplica las migraciones pendientes al iniciar. En un contenedor recién levantado
// esto crea la base de datos, su esquema y los datos iniciales sin intervención
// manual. Se reintenta porque el servidor SQL del contenedor puede tardar en aceptar
// conexiones cuando ambos servicios arrancan a la vez.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    for (var intento = 1; intento <= 12; intento++)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex) when (intento < 12)
        {
            app.Logger.LogWarning("Base de datos no disponible (intento {Intento}): {Mensaje}", intento, ex.Message);
            Thread.Sleep(5000);
        }
    }
}

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Sirve los archivos subidos en tiempo de ejecución (portadas en wwwroot/images);
// MapStaticAssets solo entrega los archivos conocidos al compilar.
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
