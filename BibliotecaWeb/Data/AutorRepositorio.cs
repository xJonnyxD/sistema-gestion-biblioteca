using BibliotecaWeb.Models;

namespace BibliotecaWeb.Data;

/// <summary>
/// Almacén de autores en memoria. Conserva los datos mientras la aplicación
/// está en ejecución, de modo que las operaciones de edición y eliminación se
/// reflejan en el listado. Cuando se incorpore Entity Framework, esta clase se
/// reemplaza por el acceso a la base de datos sin tocar controladores ni vistas.
/// </summary>
public static class AutorRepositorio
{
    private static readonly List<Autor> _autores = new()
    {
        new Autor { Id = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927, 3, 6), Activo = false },
        new Autor { Id = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2), Activo = true },
        new Autor { Id = 3, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936, 3, 28), Activo = true },
        new Autor { Id = 4, Nombre = "Miguel", Apellido = "de Cervantes", Nacionalidad = "Española", FechaNacimiento = new DateTime(1547, 9, 29), Activo = false },
        new Autor { Id = 5, Nombre = "Claribel", Apellido = "Alegría", Nacionalidad = "Salvadoreña", FechaNacimiento = new DateTime(1924, 5, 12), Activo = true }
    };

    public static IReadOnlyList<Autor> Listar() => _autores;

    public static Autor? Obtener(int id) => _autores.FirstOrDefault(a => a.Id == id);

    public static void Actualizar(Autor autor)
    {
        var actual = Obtener(autor.Id);
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

    public static void Eliminar(int id)
    {
        var autor = Obtener(id);
        if (autor is not null)
        {
            _autores.Remove(autor);
        }
    }
}
