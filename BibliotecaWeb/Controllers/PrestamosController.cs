using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class PrestamosController : Controller
{
    public IActionResult Index()
    {
        var prestamos = new List<Prestamo>
        {
            new Prestamo
            {
                Id = 1,
                Libro = "Cien años de soledad",
                Usuario = "Ana Martínez",
                FechaPrestamo = new DateTime(2026, 7, 10),
                FechaDevolucion = new DateTime(2026, 7, 17),
                Estado = EstadoPrestamo.Devuelto
            },
            new Prestamo
            {
                Id = 2,
                Libro = "La casa de los espíritus",
                Usuario = "Carlos Rivas",
                FechaPrestamo = new DateTime(2026, 7, 15),
                FechaDevolucion = new DateTime(2026, 7, 22),
                Estado = EstadoPrestamo.Pendiente
            },
            new Prestamo
            {
                Id = 3,
                Libro = "Don Quijote de la Mancha",
                Usuario = "Lucía Hernández",
                FechaPrestamo = new DateTime(2026, 7, 1),
                FechaDevolucion = new DateTime(2026, 7, 8),
                Estado = EstadoPrestamo.Atrasado
            },
            new Prestamo
            {
                Id = 4,
                Libro = "Cenizas de Izalco",
                Usuario = "Roberto Cruz",
                FechaPrestamo = new DateTime(2026, 7, 16),
                FechaDevolucion = new DateTime(2026, 7, 23),
                Estado = EstadoPrestamo.Pendiente
            }
        };

        return View(prestamos);
    }
}
