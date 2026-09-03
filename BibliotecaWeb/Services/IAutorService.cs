using BibliotecaWeb.Models;

namespace BibliotecaWeb.Services;

/// <summary>
/// Define las operaciones para trabajar con la información de los autores.
/// El controlador depende de esta abstracción (no de una implementación concreta),
/// lo que permite aplicar Inversión de Control e Inyección de Dependencias.
/// </summary>
public interface IAutorService
{
    IEnumerable<Autor> ObtenerTodos();
    Autor? ObtenerPorId(int id);
    void Actualizar(Autor autor);
    void Eliminar(int id);
}
