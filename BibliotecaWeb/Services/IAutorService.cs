using BibliotecaWeb.Models;

namespace BibliotecaWeb.Services;

public interface IAutorService
{
    IEnumerable<Autor> ObtenerTodos();
    Autor? ObtenerPorId(int id);
    void Actualizar(Autor autor);
    void Eliminar(int id);
}
