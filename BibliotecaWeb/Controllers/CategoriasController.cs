using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class CategoriasController : Controller
{
    public IActionResult Index()
    {
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

        return View(categorias);
    }
}
