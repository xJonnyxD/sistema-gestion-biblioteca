using BibliotecaWeb.Models;

namespace BibliotecaWeb.Services;

public interface ILibroService
{
    List<Libro> ObtenerTodos();
    List<Libro> Buscar(string termino);
    Libro? ObtenerPorId(int id);
    void Crear(Libro libro);
    bool Actualizar(Libro libro);
    bool Eliminar(int id);
}
