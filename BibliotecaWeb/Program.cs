using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BibliotecaWeb.Data;
using BibliotecaWeb.Services;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDB")));

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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccesoDenegado";
});

builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddScoped<ILibroService, LibroService>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

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

    foreach (var rol in new[] { "Administrador", "Lector" })
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
            await userManager.AddToRoleAsync(admin, "Administrador");
        }
    }
    else if (!await userManager.IsInRoleAsync(admin, "Administrador"))
    {
        await userManager.AddToRoleAsync(admin, "Administrador");
    }
}

app.UseForwardedHeaders();

app.UseExceptionHandler("/Home/Error");

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

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
