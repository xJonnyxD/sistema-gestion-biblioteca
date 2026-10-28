using BibliotecaWeb.Models;

namespace BibliotecaWeb.Services;

/// <summary>
/// Implementación de <see cref="IAutorService"/>. Contiene la lógica de gestión y
/// consulta de los autores. Los datos se mantienen en una lista en memoria
/// (compartida entre peticiones) mientras la aplicación está en ejecución.
/// </summary>
public class AutorService : IAutorService
{
    private static readonly List<Autor> _autores = new()
    {
        new Autor { Id = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927, 3, 6), Activo = false },
        new Autor { Id = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2), Activo = true },
        new Autor { Id = 3, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936, 3, 28), Activo = true },
        new Autor { Id = 4, Nombre = "Miguel", Apellido = "de Cervantes", Nacionalidad = "Española", FechaNacimiento = new DateTime(1547, 9, 29), Activo = false },
        new Autor { Id = 5, Nombre = "Claribel", Apellido = "Alegría", Nacionalidad = "Salvadoreña", FechaNacimiento = new DateTime(1924, 5, 12), Activo = true }
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
