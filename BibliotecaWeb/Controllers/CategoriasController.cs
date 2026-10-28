using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class CategoriasController : Controller
{
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ILogger<CategoriasController> logger)
    {
        _logger = logger;
    }

    private static List<Categoria> ObtenerCategorias() => new()
    {
        new Categoria { Id = 1, Nombre = "Novela", Descripcion = "Obras narrativas de ficción.", Activa = true },
        new Categoria { Id = 2, Nombre = "Infantil", Descripcion = "Material dirigido al público infantil.", Activa = true },
        new Categoria { Id = 3, Nombre = "Clásico", Descripcion = "Obras de la literatura universal.", Activa = true },
        new Categoria { Id = 4, Nombre = "Referencia", Descripcion = "Diccionarios y enciclopedias de consulta en sala.", Activa = false }
    };

    public IActionResult Index()
    {
        _logger.LogInformation("Consultando listado de categorías.");

        try
        {
            var categorias = ObtenerCategorias();
            _logger.LogInformation("Se cargaron {Cantidad} categoría(s).", categorias.Count);
            return View(categorias);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el listado de categorías.");
            TempData["Error"] = "Ocurrió un error al cargar las categorías.";
            return View(new List<Categoria>());
        }
    }

    public IActionResult Detalle(int id)
    {
        _logger.LogInformation("Consultando detalle de la categoría con Id {CategoriaId}.", id);

        var categoria = ObtenerCategorias().FirstOrDefault(c => c.Id == id);
        if (categoria is null)
        {
            _logger.LogWarning("No se encontró la categoría con Id {CategoriaId}.", id);
            return NotFound();
        }

        _logger.LogInformation("Mostrando detalle de la categoría «{Nombre}» (Id: {CategoriaId}).", categoria.Nombre, categoria.Id);
        return View(categoria);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public IActionResult Crear()
    {
        _logger.LogInformation("Accediendo al formulario de creación de categoría.");
        return View(new Categoria());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Crear(Categoria categoria)
    {
        _logger.LogInformation("Iniciando creación de categoría «{Nombre}».", categoria.Nombre);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido al intentar crear categoría «{Nombre}».", categoria.Nombre);
            return View(categoria);
        }

        _logger.LogInformation("Categoría «{Nombre}» registrada exitosamente.", categoria.Nombre);
        TempData["Exito"] = $"La categoría «{categoria.Nombre}» se agregó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public IActionResult Editar(int id)
    {
        _logger.LogInformation("Accediendo al formulario de edición de la categoría con Id {CategoriaId}.", id);

        var categoria = ObtenerCategorias().FirstOrDefault(c => c.Id == id);
        if (categoria is null)
        {
            _logger.LogWarning("No se encontró la categoría con Id {CategoriaId} para editar.", id);
            return NotFound();
        }

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Editar(Categoria categoria)
    {
        _logger.LogInformation("Iniciando edición de la categoría con Id {CategoriaId}.", categoria.Id);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido al intentar editar categoría con Id {CategoriaId}.", categoria.Id);
            return View(categoria);
        }

        _logger.LogInformation("Categoría «{Nombre}» (Id: {CategoriaId}) actualizada exitosamente.", categoria.Nombre, categoria.Id);
        TempData["Exito"] = $"La categoría «{categoria.Nombre}» se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        _logger.LogInformation("Accediendo a confirmación de eliminación de la categoría con Id {CategoriaId}.", id);

        var categoria = ObtenerCategorias().FirstOrDefault(c => c.Id == id);
        if (categoria is null)
        {
            _logger.LogWarning("No se encontró la categoría con Id {CategoriaId} para eliminar.", id);
            return NotFound();
        }

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult EliminarConfirmado(int id)
    {
        _logger.LogInformation("Iniciando eliminación de la categoría con Id {CategoriaId}.", id);

        var categoria = ObtenerCategorias().FirstOrDefault(c => c.Id == id);
        if (categoria is null)
        {
            _logger.LogWarning("No se encontró la categoría con Id {CategoriaId} al confirmar eliminación.", id);
            return RedirectToAction(nameof(Index));
        }

        _logger.LogInformation("Categoría «{Nombre}» (Id: {CategoriaId}) eliminada exitosamente.", categoria.Nombre, categoria.Id);
        TempData["Exito"] = $"La categoría «{categoria.Nombre}» se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
