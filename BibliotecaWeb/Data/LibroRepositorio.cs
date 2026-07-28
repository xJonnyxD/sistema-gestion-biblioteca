using BibliotecaWeb.Models;

namespace BibliotecaWeb.Data;

/// <summary>
/// Almacén de libros en memoria. Mantiene el catálogo y genera los identificadores
/// de los libros nuevos mientras la aplicación está en ejecución.
/// </summary>
public static class LibroRepositorio
{
    private static readonly List<Libro> _libros = new()
    {
        new Libro { Id = 1, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Categoria = "Novela", AnioPublicacion = 1967, Disponibles = 3, Descripcion = "La saga de la familia Buendía en el pueblo de Macondo, obra cumbre del realismo mágico.", ImagenNombre = "cien-anios-de-soledad.svg" },
        new Libro { Id = 2, Titulo = "La casa de los espíritus", Autor = "Isabel Allende", Categoria = "Novela", AnioPublicacion = 1982, Disponibles = 2, Descripcion = "Tres generaciones de la familia Trueba en un país latinoamericano marcado por el cambio.", ImagenNombre = "la-casa-de-los-espiritus.svg" },
        new Libro { Id = 3, Titulo = "La ciudad y los perros", Autor = "Mario Vargas Llosa", Categoria = "Novela", AnioPublicacion = 1963, Disponibles = 0, Descripcion = "La vida de un grupo de cadetes en un colegio militar de Lima.", ImagenNombre = "la-ciudad-y-los-perros.svg" },
        new Libro { Id = 4, Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", Categoria = "Clásico", AnioPublicacion = 1605, Disponibles = 1, Descripcion = "Las aventuras del ingenioso hidalgo y su escudero Sancho Panza.", ImagenNombre = "don-quijote-de-la-mancha.svg" },
        new Libro { Id = 5, Titulo = "Cenizas de Izalco", Autor = "Claribel Alegría", Categoria = "Novela", AnioPublicacion = 1966, Disponibles = 0, Descripcion = "Una historia de amor enmarcada en la matanza salvadoreña de 1932.", ImagenNombre = "cenizas-de-izalco.svg" }
    };

    public static IReadOnlyList<Libro> Listar() => _libros;

    public static Libro? Obtener(int id) => _libros.FirstOrDefault(l => l.Id == id);

    public static void Agregar(Libro libro)
    {
        libro.Id = _libros.Count == 0 ? 1 : _libros.Max(l => l.Id) + 1;
        _libros.Add(libro);
    }

    public static void Actualizar(Libro libro)
    {
        var actual = Obtener(libro.Id);
        if (actual is null)
        {
            return;
        }

        actual.Titulo = libro.Titulo;
        actual.Autor = libro.Autor;
        actual.Categoria = libro.Categoria;
        actual.AnioPublicacion = libro.AnioPublicacion;
        actual.Disponibles = libro.Disponibles;
        actual.Descripcion = libro.Descripcion;
        actual.ImagenNombre = libro.ImagenNombre;
    }

    public static void Eliminar(int id)
    {
        var libro = Obtener(id);
        if (libro is not null)
        {
            _libros.Remove(libro);
        }
    }
}
