using BibliotecaWeb.Data;
using BibliotecaWeb.Models;

namespace BibliotecaWeb.Services;

public class LibroService : ILibroService
{
    private readonly ApplicationDbContext _context;

    public LibroService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Libro> ObtenerTodos()
    {
        return _context.Libros.OrderBy(l => l.Titulo).ToList();
    }

    public List<Libro> Buscar(string termino)
    {
        var t = termino.Trim();
        return _context.Libros
            .Where(l => l.Titulo.Contains(t) ||
                        l.Autor.Contains(t) ||
                        l.Categoria.Contains(t))
            .OrderBy(l => l.Titulo)
            .ToList();
    }

    public Libro? ObtenerPorId(int id)
    {
        return _context.Libros.Find(id);
    }

    public void Crear(Libro libro)
    {
        _context.Libros.Add(libro);
        _context.SaveChanges();
    }

    public bool Actualizar(Libro libro)
    {
        var actual = _context.Libros.Find(libro.Id);
        if (actual is null) return false;

        actual.Titulo = libro.Titulo;
        actual.Autor = libro.Autor;
        actual.Categoria = libro.Categoria;
        actual.AnioPublicacion = libro.AnioPublicacion;
        actual.Disponibles = libro.Disponibles;
        actual.Descripcion = libro.Descripcion;

        _context.Libros.Update(actual);
        _context.SaveChanges();
        return true;
    }

    public bool Eliminar(int id)
    {
        var libro = _context.Libros.Find(id);
        if (libro is null) return false;

        _context.Libros.Remove(libro);
        _context.SaveChanges();
        return true;
    }
}
