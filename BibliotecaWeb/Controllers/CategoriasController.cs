using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class CategoriasController : Controller
{
    private readonly CategoriaRepositorio _repositorio;

    public CategoriasController(CategoriaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    // Mostrar categorías
    public IActionResult Index()
    {
        return View(_repositorio.Listar());
    }

    // Agregar categoría
    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Categoria { Activa = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Categoria categoria)
    {
        if (!ModelState.IsValid)
        {
            return View(categoria);
        }

        _repositorio.Agregar(categoria);
        return RedirectToAction(nameof(Index));
    }

    // Editar categoría
    [HttpGet]
    public IActionResult Editar(int id)
    {
        var categoria = _repositorio.Obtener(id);
        if (categoria is null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Categoria categoria)
    {
        if (!ModelState.IsValid)
        {
            return View(categoria);
        }

        _repositorio.Actualizar(categoria);
        return RedirectToAction(nameof(Index));
    }

    // Eliminar categoría
    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var categoria = _repositorio.Obtener(id);
        if (categoria is null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarConfirmado(int id)
    {
        _repositorio.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }
}
