using BibliotecaWeb.Models;

namespace BibliotecaWeb.Services;

/// <summary>
/// Segunda implementación de <see cref="IAutorService"/> (modo demostración) con un
/// conjunto de autores diferente. Sirve para comprobar la Inversión de Control:
/// basta con cambiar el registro en Program.cs para que el controlador use esta
/// implementación, sin modificar el AutoresController.
/// </summary>
public class AutorServiceDemo : IAutorService
{
    private static readonly List<Autor> _autores = new()
    {
        new Autor { Id = 1, Nombre = "Jorge Luis", Apellido = "Borges", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1899, 8, 24), Activo = true },
        new Autor { Id = 2, Nombre = "Julio", Apellido = "Cortázar", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1914, 8, 26), Activo = true },
        new Autor { Id = 3, Nombre = "Octavio", Apellido = "Paz", Nacionalidad = "Mexicana", FechaNacimiento = new DateTime(1914, 3, 31), Activo = false }
    };

    public IEnumerable<Autor> ObtenerTodos() => _autores;

    public Autor? ObtenerPorId(int id) => _autores.FirstOrDefault(a => a.Id == id);

    public void Actualizar(Autor autor)
    {
        var actual = ObtenerPorId(autor.Id);
        if (actual is null)
        {
            return;
        }

        actual.Nombre = autor.Nombre;
        actual.Apellido = autor.Apellido;
        actual.Nacionalidad = autor.Nacionalidad;
        actual.FechaNacimiento = autor.FechaNacimiento;
        actual.Activo = autor.Activo;
    }

    public void Eliminar(int id)
    {
        var autor = ObtenerPorId(id);
        if (autor is not null)
        {
            _autores.Remove(autor);
        }
    }
}
