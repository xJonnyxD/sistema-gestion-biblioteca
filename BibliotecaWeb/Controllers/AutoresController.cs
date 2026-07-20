using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class AutoresController : Controller
{
    public IActionResult Index()
    {
        var autores = new List<Autor>
        {
            new Autor
            {
                Id = 1,
                Nombre = "Gabriel",
                Apellido = "García Márquez",
                Nacionalidad = "Colombiana",
                FechaNacimiento = new DateTime(1927, 3, 6),
                Activo = false
            },
            new Autor
            {
                Id = 2,
                Nombre = "Isabel",
                Apellido = "Allende",
                Nacionalidad = "Chilena",
                FechaNacimiento = new DateTime(1942, 8, 2),
                Activo = true
            },
            new Autor
            {
                Id = 3,
                Nombre = "Mario",
                Apellido = "Vargas Llosa",
                Nacionalidad = "Peruana",
                FechaNacimiento = new DateTime(1936, 3, 28),
                Activo = true
            },
            new Autor
            {
                Id = 4,
                Nombre = "Miguel",
                Apellido = "de Cervantes",
                Nacionalidad = "Española",
                FechaNacimiento = new DateTime(1547, 9, 29),
                Activo = false
            },
            new Autor
            {
                Id = 5,
                Nombre = "Claribel",
                Apellido = "Alegría",
                Nacionalidad = "Salvadoreña",
                FechaNacimiento = new DateTime(1924, 5, 12),
                Activo = true
            }
        };

        return View(autores);
    }
}
