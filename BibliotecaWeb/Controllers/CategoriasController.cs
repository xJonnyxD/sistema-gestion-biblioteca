using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class CategoriasController : Controller
{
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ILogger<CategoriasController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Consultando listado de categorías.");
        var categorias = new List<Categoria>
        {
            new Categoria
            {
                Id = 1,
                Nombre = "Novela",
                Descripcion = "Obras narrativas de ficción.",
                Activa = true
            },
            new Categoria
            {
                Id = 2,
                Nombre = "Infantil",
                Descripcion = "Material dirigido al público infantil.",
                Activa = true
            },
            new Categoria
            {
                Id = 3,
                Nombre = "Clásico",
                Descripcion = "Obras de la literatura universal.",
                Activa = true
            },
            new Categoria
            {
                Id = 4,
                Nombre = "Referencia",
                Descripcion = "Diccionarios y enciclopedias de consulta en sala.",
                Activa = false
            }
        };

        _logger.LogInformation("Se cargaron {Cantidad} categoría(s).", categorias.Count);
        return View(categorias);
    }
}
