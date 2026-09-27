using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

/// <summary>
/// Gestiona la autenticación de usuarios (registro, inicio y cierre de sesión)
/// mediante ASP.NET Core Identity: UserManager crea y consulta usuarios y
/// SignInManager valida credenciales y administra la sesión.
/// </summary>
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // REGISTRO ---------------------------------------------------------------

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var usuario = new IdentityUser
        {
            UserName = modelo.Usuario,
            Email = modelo.Correo
        };

        var resultado = await _userManager.CreateAsync(usuario, modelo.Password);
        if (resultado.Succeeded)
        {
            // Inicia sesión automáticamente tras un registro correcto.
            await _signInManager.SignInAsync(usuario, isPersistent: false);
            TempData["Exito"] = $"¡Bienvenido, {usuario.UserName}! Tu cuenta se creó correctamente.";
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in resultado.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(modelo);
    }

    // LOGIN ------------------------------------------------------------------

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel modelo, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        // Permite ingresar con usuario o con correo: si el dato contiene "@" se
        // busca por correo; en cualquier caso se resuelve el nombre de usuario.
        var entrada = modelo.UsuarioOCorreo.Trim();
        var usuario = entrada.Contains('@')
            ? await _userManager.FindByEmailAsync(entrada)
            : await _userManager.FindByNameAsync(entrada);

        if (usuario is not null)
        {
            // SignInManager valida las credenciales y crea la cookie de sesión.
            var resultado = await _signInManager.PasswordSignInAsync(
                usuario.UserName!, modelo.Password, modelo.Recordarme, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
        }

        // Mensaje único para credenciales inválidas (no revela si el usuario existe).
        ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
        return View(modelo);
    }

    // CERRAR SESIÓN ----------------------------------------------------------

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // SignInManager elimina la cookie de sesión del usuario.
        await _signInManager.SignOutAsync();
        TempData["Exito"] = "Sesión cerrada correctamente.";
        return RedirectToAction("Index", "Home");
    }
}
