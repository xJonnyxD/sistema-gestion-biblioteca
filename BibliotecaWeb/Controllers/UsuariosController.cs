using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class UsuariosController : Controller
{
    public IActionResult Index()
    {
        var usuarios = new List<Usuario>
        {
            new Usuario
            {
                Id = 1,
                Carne = "BIB-001",
                Nombre = "Ana",
                Apellido = "Martínez",
                Correo = "ana.martinez@biblioteca.edu.sv",
                Activo = true
            },
            new Usuario
            {
                Id = 2,
                Carne = "BIB-002",
                Nombre = "Carlos",
                Apellido = "Rivas",
                Correo = "carlos.rivas@biblioteca.edu.sv",
                Activo = true
            },
            new Usuario
            {
                Id = 3,
                Carne = "BIB-003",
                Nombre = "Lucía",
                Apellido = "Hernández",
                Correo = "lucia.hernandez@biblioteca.edu.sv",
                Activo = false
            },
            new Usuario
            {
                Id = 4,
                Carne = "BIB-004",
                Nombre = "Roberto",
                Apellido = "Cruz",
                Correo = "roberto.cruz@biblioteca.edu.sv",
                Activo = true
            }
        };

        return View(usuarios);
    }
}
