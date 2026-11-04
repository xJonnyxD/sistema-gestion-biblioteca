using BibliotecaWeb.Models;
using BibliotecaWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

[Authorize]
public class LibrosController : Controller
{
    private const string ImagenPorDefecto = "sin-imagen.svg";
    private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

    private readonly ILibroService _libroService;
    private readonly IWebHostEnvironment _entorno;
    private readonly ILogger<LibrosController> _logger;

    public LibrosController(ILibroService libroService, IWebHostEnvironment entorno, ILogger<LibrosController> logger)
    {
        _libroService = libroService;
        _entorno = entorno;
        _logger = logger;
    }

    public IActionResult Index(string? buscar)
    {
        _logger.LogInformation("Consultando listado de libros. Término de búsqueda: {Buscar}", buscar ?? "(ninguno)");

        var libros = string.IsNullOrWhiteSpace(buscar)
            ? _libroService.ObtenerTodos()
            : _libroService.Buscar(buscar);

        _logger.LogInformation("Se encontraron {Cantidad} libro(s) en la consulta.", libros.Count);

        ViewData["Buscar"] = buscar;
        return View(libros);
    }

    public IActionResult Detalle(int id)
    {
        _logger.LogInformation("Consultando detalle del libro con Id {LibroId}.", id);

        var libro = _libroService.ObtenerPorId(id);
        if (libro is null)
        {
            _logger.LogWarning("No se encontró el libro con Id {LibroId}.", id);
            return NotFound();
        }

        _logger.LogInformation("Mostrando detalle del libro «{Titulo}» (Id: {LibroId}).", libro.Titulo, libro.Id);
        return View(libro);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public IActionResult Crear()
    {
        _logger.LogInformation("Accediendo al formulario de creación de libro.");
        return View(new Libro());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Crear(Libro libro, IFormFile? imagen)
    {
        _logger.LogInformation("Iniciando creación de libro «{Titulo}».", libro.Titulo);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido al intentar crear libro «{Titulo}».", libro.Titulo);
            return View(libro);
        }

        try
        {
            libro.ImagenNombre = GuardarImagen(imagen) ?? ImagenPorDefecto;

            _libroService.Crear(libro);

            _logger.LogInformation("Libro «{Titulo}» creado exitosamente con Id {LibroId}.", libro.Titulo, libro.Id);
            TempData["Exito"] = $"El libro «{libro.Titulo}» se agregó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el libro «{Titulo}».", libro.Titulo);
            TempData["Error"] = "Ocurrió un error al guardar el libro. Intente nuevamente.";
            return View(libro);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public IActionResult Editar(int id)
    {
        _logger.LogInformation("Accediendo al formulario de edición del libro con Id {LibroId}.", id);

        var libro = _libroService.ObtenerPorId(id);
        if (libro is null)
        {
            _logger.LogWarning("No se encontró el libro con Id {LibroId} para editar.", id);
            return NotFound();
        }

        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Editar(Libro libro, IFormFile? imagen)
    {
        _logger.LogInformation("Iniciando edición del libro con Id {LibroId}.", libro.Id);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido al intentar editar libro con Id {LibroId}.", libro.Id);
            return View(libro);
        }

        try
        {
            var actualizado = _libroService.Actualizar(libro);
            if (!actualizado)
            {
                _logger.LogWarning("No se encontró el libro con Id {LibroId} en la base de datos al editar.", libro.Id);
                return NotFound();
            }

            _logger.LogInformation("Libro «{Titulo}» (Id: {LibroId}) actualizado exitosamente.", libro.Titulo, libro.Id);
            TempData["Exito"] = $"El libro «{libro.Titulo}» se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el libro con Id {LibroId}.", libro.Id);
            TempData["Error"] = "Ocurrió un error al actualizar el libro. Intente nuevamente.";
            return View(libro);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        _logger.LogInformation("Accediendo a confirmación de eliminación del libro con Id {LibroId}.", id);

        var libro = _libroService.ObtenerPorId(id);
        if (libro is null)
        {
            _logger.LogWarning("No se encontró el libro con Id {LibroId} para eliminar.", id);
            return NotFound();
        }

        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult EliminarConfirmado(int id)
    {
        _logger.LogInformation("Iniciando eliminación del libro con Id {LibroId}.", id);

        try
        {
            var eliminado = _libroService.Eliminar(id);
            if (!eliminado)
            {
                _logger.LogWarning("No se encontró el libro con Id {LibroId} al confirmar eliminación.", id);
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Libro con Id {LibroId} eliminado exitosamente.", id);
            TempData["Exito"] = "El libro se eliminó correctamente.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el libro con Id {LibroId}.", id);
            TempData["Error"] = "Ocurrió un error al eliminar el libro. Intente nuevamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    private string? GuardarImagen(IFormFile? imagen)
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
        imagen.CopyTo(stream);

        return nombreArchivo;
    }
}
