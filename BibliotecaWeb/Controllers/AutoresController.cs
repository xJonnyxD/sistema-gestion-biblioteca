using BibliotecaWeb.Models;
using BibliotecaWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class AutoresController : Controller
{
    private readonly IAutorService _autorService;

    // El controlador recibe la dependencia por su constructor (Inyección de Dependencias).
    // No crea instancias de AutorService: depende únicamente de la abstracción IAutorService.
    public AutoresController(IAutorService autorService)
    {
        _autorService = autorService;
    }

    public IActionResult Index()
    {
        return View(_autorService.ObtenerTodos());
    }

    [HttpGet]
    public IActionResult Detalle(int id)
    {
        var autor = _autorService.ObtenerPorId(id);
        if (autor is null)
        {
            return NotFound();
        }

        return View(autor);
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var autor = _autorService.ObtenerPorId(id);
        if (autor is null)
        {
            return NotFound();
        }

        return View(autor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Autor autor)
    {
        if (!ModelState.IsValid)
        {
            return View(autor);
        }

        _autorService.Actualizar(autor);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var autor = _autorService.ObtenerPorId(id);
        if (autor is null)
        {
            return NotFound();
        }

        return View(autor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarConfirmado(int id)
    {
        _autorService.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }
}
