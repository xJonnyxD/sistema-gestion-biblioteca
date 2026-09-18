using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BibliotecaWeb.Migrations
{
    /// <inheritdoc />
    public partial class InicialLibros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: false),
                    Disponibles = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagenNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "Id", "AnioPublicacion", "Autor", "Categoria", "Descripcion", "Disponibles", "ImagenNombre", "Titulo" },
                values: new object[,]
                {
                    { 1, 1967, "Gabriel García Márquez", "Novela", "La saga de la familia Buendía en el pueblo de Macondo, obra cumbre del realismo mágico.", 3, "cien-anios-de-soledad.svg", "Cien años de soledad" },
                    { 2, 1982, "Isabel Allende", "Novela", "Tres generaciones de la familia Trueba en un país latinoamericano marcado por el cambio.", 2, "la-casa-de-los-espiritus.svg", "La casa de los espíritus" },
                    { 3, 1963, "Mario Vargas Llosa", "Novela", "La vida de un grupo de cadetes en un colegio militar de Lima.", 0, "la-ciudad-y-los-perros.svg", "La ciudad y los perros" },
                    { 4, 1605, "Miguel de Cervantes", "Clásico", "Las aventuras del ingenioso hidalgo y su escudero Sancho Panza.", 1, "don-quijote-de-la-mancha.svg", "Don Quijote de la Mancha" },
                    { 5, 1966, "Claribel Alegría", "Novela", "Una historia de amor enmarcada en la matanza salvadoreña de 1932.", 0, "cenizas-de-izalco.svg", "Cenizas de Izalco" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Libros");
        }
    }
}
