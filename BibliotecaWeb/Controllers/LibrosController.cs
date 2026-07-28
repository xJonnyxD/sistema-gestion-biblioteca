using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class LibrosController : Controller
{
    private const string ImagenPorDefecto = "sin-imagen.svg";
    private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

    private readonly IWebHostEnvironment _entorno;

    public LibrosController(IWebHostEnvironment entorno)
    {
        _entorno = entorno;
    }

    public IActionResult Index()
    {
        return View(LibroRepositorio.Listar());
    }

    public IActionResult Detalle(int id)
    {
        var libro = LibroRepositorio.Obtener(id);
        if (libro is null)
        {
            return NotFound();
        }

        return View(libro);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Libro());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Libro libro, IFormFile? imagen)
    {
        if (!ModelState.IsValid)
        {
            return View(libro);
        }

        libro.ImagenNombre = await GuardarImagenAsync(imagen) ?? ImagenPorDefecto;
        LibroRepositorio.Agregar(libro);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var libro = LibroRepositorio.Obtener(id);
        if (libro is null)
        {
            return NotFound();
        }

        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Libro libro, IFormFile? imagen)
    {
        if (!ModelState.IsValid)
        {
            return View(libro);
        }

        var nuevaImagen = await GuardarImagenAsync(imagen);
        if (nuevaImagen is not null)
        {
            libro.ImagenNombre = nuevaImagen;
        }

        LibroRepositorio.Actualizar(libro);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var libro = LibroRepositorio.Obtener(id);
        if (libro is null)
        {
            return NotFound();
        }

        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarConfirmado(int id)
    {
        LibroRepositorio.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Guarda la imagen recibida en wwwroot/images con un nombre único y devuelve
    /// ese nombre. Devuelve null si no se envió ningún archivo o si la extensión
    /// no está permitida, para que el llamador conserve la imagen previa.
    /// </summary>
    private async Task<string?> GuardarImagenAsync(IFormFile? imagen)
    {
        if (imagen is null || imagen.Length == 0)
        {
            return null;
        }

        var extension = Path.GetExtension(imagen.FileName).ToLowerInvariant();
        if (!ExtensionesPermitidas.Contains(extension))
        {
            return null;
        }

        var carpeta = Path.Combine(_entorno.WebRootPath, "images");
        Directory.CreateDirectory(carpeta);

        var nombreArchivo = $"{Guid.NewGuid():N}{extension}";
        var ruta = Path.Combine(carpeta, nombreArchivo);

        using var stream = new FileStream(ruta, FileMode.Create);
        await imagen.CopyToAsync(stream);

        return nombreArchivo;
    }
}
