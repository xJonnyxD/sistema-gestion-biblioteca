using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaWeb.Migrations
{
    /// <inheritdoc />
    public partial class PortadasLibros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagenNombre",
                value: "cien-anios-de-soledad.jpg");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagenNombre",
                value: "la-casa-de-los-espiritus.jpg");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagenNombre",
                value: "la-ciudad-y-los-perros.jpg");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagenNombre",
                value: "don-quijote-de-la-mancha.jpg");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagenNombre",
                value: "cenizas-de-izalco.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagenNombre",
                value: "cien-anios-de-soledad.svg");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagenNombre",
                value: "la-casa-de-los-espiritus.svg");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagenNombre",
                value: "la-ciudad-y-los-perros.svg");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagenNombre",
                value: "don-quijote-de-la-mancha.svg");

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagenNombre",
                value: "cenizas-de-izalco.svg");
        }
    }
}
