using BibliotecaWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Data;

/// <summary>
/// Contexto de Entity Framework Core. Representa la sesión con la base de datos
/// SQL Server y expone cada entidad del dominio a través de su <see cref="DbSet{TEntity}"/>.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Tabla de libros. Cada elemento del DbSet se corresponde con una fila de la
    /// tabla Libros en la base de datos.
    /// </summary>
    public DbSet<Libro> Libros => Set<Libro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Datos iniciales (seed): se insertan al aplicar la migración, de modo que
        // la tabla Libros no quede vacía la primera vez que se ejecuta la aplicación.
        modelBuilder.Entity<Libro>().HasData(
            new Libro { Id = 1, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Categoria = "Novela", AnioPublicacion = 1967, Disponibles = 3, Descripcion = "La saga de la familia Buendía en el pueblo de Macondo, obra cumbre del realismo mágico.", ImagenNombre = "cien-anios-de-soledad.jpg" },
            new Libro { Id = 2, Titulo = "La casa de los espíritus", Autor = "Isabel Allende", Categoria = "Novela", AnioPublicacion = 1982, Disponibles = 2, Descripcion = "Tres generaciones de la familia Trueba en un país latinoamericano marcado por el cambio.", ImagenNombre = "la-casa-de-los-espiritus.jpg" },
            new Libro { Id = 3, Titulo = "La ciudad y los perros", Autor = "Mario Vargas Llosa", Categoria = "Novela", AnioPublicacion = 1963, Disponibles = 0, Descripcion = "La vida de un grupo de cadetes en un colegio militar de Lima.", ImagenNombre = "la-ciudad-y-los-perros.jpg" },
            new Libro { Id = 4, Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", Categoria = "Clásico", AnioPublicacion = 1605, Disponibles = 1, Descripcion = "Las aventuras del ingenioso hidalgo y su escudero Sancho Panza.", ImagenNombre = "don-quijote-de-la-mancha.jpg" },
            new Libro { Id = 5, Titulo = "Cenizas de Izalco", Autor = "Claribel Alegría", Categoria = "Novela", AnioPublicacion = 1966, Disponibles = 0, Descripcion = "Una historia de amor enmarcada en la matanza salvadoreña de 1932.", ImagenNombre = "cenizas-de-izalco.jpg" }
        );
    }
}
