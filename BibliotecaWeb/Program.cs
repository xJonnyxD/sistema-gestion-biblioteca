using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BibliotecaWeb.Data;
using BibliotecaWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Entity Framework Core: registra el ApplicationDbContext y lo configura para
// comunicarse con SQL Server usando la cadena de conexión "BibliotecaDB".
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDB")));

// ASP.NET Core Identity: gestión de usuarios (IdentityUser) almacenados con
// Entity Framework Core en el ApplicationDbContext. Habilita UserManager y
// SignInManager para el registro, inicio y cierre de sesión.
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Rutas de las páginas de autenticación (cookie de sesión).
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccesoDenegado";
});

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

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    foreach (var rol in new[] { "Admin", "Bibliotecario", "Usuario" })
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }

    var correoAdmin = builder.Configuration["AdminSeed:Correo"] ?? "admin@biblioteca.local";
    var usuarioAdmin = builder.Configuration["AdminSeed:Usuario"] ?? "admin";
    var passwordAdmin = builder.Configuration["AdminSeed:Password"] ?? "Admin123";

    var admin = await userManager.FindByNameAsync(usuarioAdmin);
    if (admin is null)
    {
        admin = new IdentityUser
        {
            UserName = usuarioAdmin,
            Email = correoAdmin,
            EmailConfirmed = true
        };

        var creado = await userManager.CreateAsync(admin, passwordAdmin);
        if (creado.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
    else if (!await userManager.IsInRoleAsync(admin, "Admin"))
    {
        await userManager.AddToRoleAsync(admin, "Admin");
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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
