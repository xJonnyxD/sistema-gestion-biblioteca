using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class AutoresController : Controller
{
    public IActionResult Index()
    {
        return View(AutorRepositorio.Listar());
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var autor = AutorRepositorio.Obtener(id);
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

        AutorRepositorio.Actualizar(autor);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var autor = AutorRepositorio.Obtener(id);
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
        AutorRepositorio.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }
}
