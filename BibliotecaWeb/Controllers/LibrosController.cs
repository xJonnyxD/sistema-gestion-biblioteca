using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class LibrosController : Controller
{
    public IActionResult Index()
    {
        var libros = new List<Libro>
        {
            new Libro
            {
                Id = 1,
                Titulo = "Cien años de soledad",
                Autor = "Gabriel García Márquez",
                Categoria = "Novela",
                AnioPublicacion = 1967,
                Disponibles = 3
            },
            new Libro
            {
                Id = 2,
                Titulo = "La casa de los espíritus",
                Autor = "Isabel Allende",
                Categoria = "Novela",
                AnioPublicacion = 1982,
                Disponibles = 2
            },
            new Libro
            {
                Id = 3,
                Titulo = "La ciudad y los perros",
                Autor = "Mario Vargas Llosa",
                Categoria = "Novela",
                AnioPublicacion = 1963,
                Disponibles = 0
            },
            new Libro
            {
                Id = 4,
                Titulo = "Don Quijote de la Mancha",
                Autor = "Miguel de Cervantes",
                Categoria = "Clásico",
                AnioPublicacion = 1605,
                Disponibles = 1
            },
            new Libro
            {
                Id = 5,
                Titulo = "Cenizas de Izalco",
                Autor = "Claribel Alegría",
                Categoria = "Novela",
                AnioPublicacion = 1966,
                Disponibles = 0
            }
        };

        return View(libros);
    }
}
