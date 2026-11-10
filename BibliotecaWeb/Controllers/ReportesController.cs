using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BibliotecaWeb.Controllers;

[Authorize]
public class ReportesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var categorias = _context.Libros
            .Select(l => l.Categoria)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        ViewBag.Categorias = categorias;
        return View();
    }

    public IActionResult GenerarPDF(string? categoria)
    {
        var libros = ObtenerLibros(categoria);
        var filtroTexto = string.IsNullOrWhiteSpace(categoria) ? "Todos" : categoria;

        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(header =>
                {
                    header.Column(col =>
                    {
                        col.Item().Text("Sistema de Gestión de Biblioteca")
                            .FontSize(18).Bold().FontColor(Colors.Blue.Darken3);

                        col.Item().Text("Reporte de Libros")
                            .FontSize(14).SemiBold().FontColor(Colors.Grey.Darken2);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Categoría: {filtroTexto}")
                                .FontSize(9).FontColor(Colors.Grey.Darken1);
                            row.RelativeItem().AlignRight().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                .FontSize(9).FontColor(Colors.Grey.Darken1);
                        });

                        col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Blue.Darken3);
                    });
                });

                page.Content().PaddingTop(10).Element(content =>
                {
                    content.Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(35);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1.5f);
                                columns.ConstantColumn(45);
                                columns.ConstantColumn(55);
                            });

                            table.Header(header =>
                            {
                                var estiloCabecera = TextStyle.Default.FontSize(9).Bold().FontColor(Colors.White);

                                header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                                    .Text("ID").Style(estiloCabecera);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                                    .Text("Título").Style(estiloCabecera);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                                    .Text("Autor").Style(estiloCabecera);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                                    .Text("Categoría").Style(estiloCabecera);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5).AlignCenter()
                                    .Text("Año").Style(estiloCabecera);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5).AlignCenter()
                                    .Text("Disp.").Style(estiloCabecera);
                            });

                            var estiloFila = TextStyle.Default.FontSize(9);
                            var filaImpar = true;

                            foreach (var libro in libros)
                            {
                                var fondo = filaImpar ? Colors.White : Colors.Grey.Lighten4;

                                table.Cell().Background(fondo).Padding(4)
                                    .Text(libro.Id.ToString()).Style(estiloFila);
                                table.Cell().Background(fondo).Padding(4)
                                    .Text(libro.Titulo).Style(estiloFila);
                                table.Cell().Background(fondo).Padding(4)
                                    .Text(libro.Autor).Style(estiloFila);
                                table.Cell().Background(fondo).Padding(4)
                                    .Text(libro.Categoria).Style(estiloFila);
                                table.Cell().Background(fondo).Padding(4).AlignCenter()
                                    .Text(libro.AnioPublicacion.ToString()).Style(estiloFila);
                                table.Cell().Background(fondo).Padding(4).AlignCenter()
                                    .Text(libro.Disponibles.ToString()).Style(estiloFila);

                                filaImpar = !filaImpar;
                            }
                        });

                        col.Item().PaddingTop(15).Row(row =>
                        {
                            row.RelativeItem().Background(Colors.Grey.Lighten3).Padding(8).Text(text =>
                            {
                                text.Span("Total de libros: ").SemiBold();
                                text.Span(libros.Count.ToString());
                                text.Span("  |  Disponibles: ").SemiBold();
                                text.Span(libros.Sum(l => l.Disponibles).ToString());
                            });
                        });
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Página ").FontSize(8);
                    text.CurrentPageNumber().FontSize(8);
                    text.Span(" de ").FontSize(8);
                    text.TotalPages().FontSize(8);
                });
            });
        });

        var pdf = documento.GeneratePdf();
        return File(pdf, "application/pdf", "ReporteLibros.pdf");
    }

    public IActionResult ExportarExcel(string? categoria)
    {
        var libros = ObtenerLibros(categoria);
        var filtroTexto = string.IsNullOrWhiteSpace(categoria) ? "Todos" : categoria;

        using var workbook = new XLWorkbook();
        var hoja = workbook.Worksheets.Add("Reporte de Libros");

        hoja.Cell("A1").Value = "Sistema de Gestión de Biblioteca - Reporte de Libros";
        hoja.Range("A1:F1").Merge().Style
            .Font.SetBold(true)
            .Font.SetFontSize(14)
            .Font.SetFontColor(XLColor.White)
            .Fill.SetBackgroundColor(XLColor.DarkBlue)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        hoja.Cell("A2").Value = $"Categoría: {filtroTexto}";
        hoja.Cell("D2").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}";
        hoja.Range("A2:F2").Style.Font.SetFontSize(9).Font.SetItalic(true);

        var filaInicio = 4;
        string[] encabezados = { "ID", "Título", "Autor", "Categoría", "Año", "Disponibles" };
        for (int i = 0; i < encabezados.Length; i++)
        {
            var celda = hoja.Cell(filaInicio, i + 1);
            celda.Value = encabezados[i];
            celda.Style
                .Font.SetBold(true)
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.DarkBlue)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
        }

        var fila = filaInicio + 1;
        foreach (var libro in libros)
        {
            hoja.Cell(fila, 1).Value = libro.Id;
            hoja.Cell(fila, 2).Value = libro.Titulo;
            hoja.Cell(fila, 3).Value = libro.Autor;
            hoja.Cell(fila, 4).Value = libro.Categoria;
            hoja.Cell(fila, 5).Value = libro.AnioPublicacion;
            hoja.Cell(fila, 6).Value = libro.Disponibles;

            var rango = hoja.Range(fila, 1, fila, 6);
            rango.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Border.SetInsideBorder(XLBorderStyleValues.Thin);

            if (fila % 2 == 0)
            {
                rango.Style.Fill.SetBackgroundColor(XLColor.LightGray);
            }

            hoja.Cell(fila, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            hoja.Cell(fila, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            fila++;
        }

        fila++;
        hoja.Cell(fila, 1).Value = "Total de libros:";
        hoja.Cell(fila, 1).Style.Font.SetBold(true);
        hoja.Cell(fila, 2).Value = libros.Count;
        hoja.Cell(fila, 3).Value = "Disponibles:";
        hoja.Cell(fila, 3).Style.Font.SetBold(true);
        hoja.Cell(fila, 4).Value = libros.Sum(l => l.Disponibles);

        hoja.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ReporteLibros.xlsx");
    }

    private List<Libro> ObtenerLibros(string? categoria)
    {
        var consulta = _context.Libros.AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            consulta = consulta.Where(l => l.Categoria == categoria);
        }

        return consulta.OrderBy(l => l.Titulo).ToList();
    }
}
