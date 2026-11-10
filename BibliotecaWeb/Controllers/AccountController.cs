using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

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
            await _userManager.AddToRoleAsync(usuario, "Lector");
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

        var entrada = modelo.UsuarioOCorreo.Trim();
        var usuario = entrada.Contains('@')
            ? await _userManager.FindByEmailAsync(entrada)
            : await _userManager.FindByNameAsync(entrada);

        if (usuario is not null)
        {
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

        ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        TempData["Exito"] = "Sesión cerrada correctamente.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}
