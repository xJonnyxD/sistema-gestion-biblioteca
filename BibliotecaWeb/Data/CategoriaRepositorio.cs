using BibliotecaWeb.Models;
using Microsoft.Data.SqlClient;

namespace BibliotecaWeb.Data;

/// <summary>
/// Acceso a datos de Categorías mediante ADO.NET y SQL Server.
/// Todas las consultas usan parámetros SQL para enviar los valores de forma segura.
/// </summary>
public class CategoriaRepositorio
{
    private readonly string _cadenaConexion;

    public CategoriaRepositorio(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")!;
    }

    // Mostrar: SELECT de todas las categorías.
    public List<Categoria> Listar()
    {
        var categorias = new List<Categoria>();

        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand(
            "SELECT Id, Nombre, Descripcion, Activa FROM dbo.Categorias ORDER BY Id;", conexion);

        conexion.Open();
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            categorias.Add(new Categoria
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.IsDBNull(2) ? string.Empty : lector.GetString(2),
                Activa = lector.GetBoolean(3)
            });
        }

        return categorias;
    }

    // Obtener una categoría por su Id (consulta parametrizada).
    public Categoria? Obtener(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand(
            "SELECT Id, Nombre, Descripcion, Activa FROM dbo.Categorias WHERE Id = @Id;", conexion);
        comando.Parameters.AddWithValue("@Id", id);

        conexion.Open();
        using var lector = comando.ExecuteReader();
        if (lector.Read())
        {
            return new Categoria
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.IsDBNull(2) ? string.Empty : lector.GetString(2),
                Activa = lector.GetBoolean(3)
            };
        }

        return null;
    }

    // Agregar: INSERT parametrizado.
    public void Agregar(Categoria categoria)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand(
            "INSERT INTO dbo.Categorias (Nombre, Descripcion, Activa) VALUES (@Nombre, @Descripcion, @Activa);",
            conexion);
        comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
        comando.Parameters.AddWithValue("@Descripcion", (object?)categoria.Descripcion ?? DBNull.Value);
        comando.Parameters.AddWithValue("@Activa", categoria.Activa);

        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // Editar: UPDATE parametrizado.
    public void Actualizar(Categoria categoria)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand(
            "UPDATE dbo.Categorias SET Nombre = @Nombre, Descripcion = @Descripcion, Activa = @Activa WHERE Id = @Id;",
            conexion);
        comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
        comando.Parameters.AddWithValue("@Descripcion", (object?)categoria.Descripcion ?? DBNull.Value);
        comando.Parameters.AddWithValue("@Activa", categoria.Activa);
        comando.Parameters.AddWithValue("@Id", categoria.Id);

        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // Eliminar: DELETE parametrizado.
    public void Eliminar(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand(
            "DELETE FROM dbo.Categorias WHERE Id = @Id;", conexion);
        comando.Parameters.AddWithValue("@Id", id);

        conexion.Open();
        comando.ExecuteNonQuery();
    }
}
