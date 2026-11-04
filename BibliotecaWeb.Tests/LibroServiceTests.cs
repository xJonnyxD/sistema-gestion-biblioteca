using BibliotecaWeb.Controllers;
using BibliotecaWeb.Models;
using BibliotecaWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace BibliotecaWeb.Tests;

public class LibrosControllerTests
{
    private readonly Mock<ILibroService> _mockService;
    private readonly Mock<IWebHostEnvironment> _mockEntorno;
    private readonly Mock<ILogger<LibrosController>> _mockLogger;
    private readonly LibrosController _controller;

    public LibrosControllerTests()
    {
        _mockService = new Mock<ILibroService>();
        _mockEntorno = new Mock<IWebHostEnvironment>();
        _mockLogger = new Mock<ILogger<LibrosController>>();

        _controller = new LibrosController(
            _mockService.Object,
            _mockEntorno.Object,
            _mockLogger.Object);

        _controller.TempData = new TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<ITempDataProvider>());
    }

    // ========================================================================
    // INDEX - Consulta de libros
    // ========================================================================

    [Fact]
    public void Index_SinBusqueda_DevuelveVistaConTodosLosLibros()
    {
        var libros = new List<Libro>
        {
            new Libro { Id = 1, Titulo = "Libro A", Autor = "Autor A", Categoria = "Novela" },
            new Libro { Id = 2, Titulo = "Libro B", Autor = "Autor B", Categoria = "Clásico" }
        };
        _mockService.Setup(s => s.ObtenerTodos()).Returns(libros);

        var resultado = _controller.Index(null) as ViewResult;

        Assert.NotNull(resultado);
        var modelo = Assert.IsAssignableFrom<List<Libro>>(resultado.Model);
        Assert.Equal(2, modelo.Count);
        _mockService.Verify(s => s.ObtenerTodos(), Times.Once);
    }

    [Fact]
    public void Index_ConBusqueda_UsaMetodoBuscar()
    {
        var libros = new List<Libro>
        {
            new Libro { Id = 1, Titulo = "Cien años de soledad", Autor = "García Márquez", Categoria = "Novela" }
        };
        _mockService.Setup(s => s.Buscar("soledad")).Returns(libros);

        var resultado = _controller.Index("soledad") as ViewResult;

        Assert.NotNull(resultado);
        var modelo = Assert.IsAssignableFrom<List<Libro>>(resultado.Model);
        Assert.Single(modelo);
        _mockService.Verify(s => s.Buscar("soledad"), Times.Once);
    }

    [Fact]
    public void Index_SinResultados_DevuelveListaVacia()
    {
        _mockService.Setup(s => s.Buscar("xyz")).Returns(new List<Libro>());

        var resultado = _controller.Index("xyz") as ViewResult;

        Assert.NotNull(resultado);
        var modelo = Assert.IsAssignableFrom<List<Libro>>(resultado.Model);
        Assert.Empty(modelo);
    }

    // ========================================================================
    // DETALLE - Consulta individual
    // ========================================================================

    [Fact]
    public void Detalle_LibroExiste_DevuelveVistaConLibro()
    {
        var libro = new Libro { Id = 1, Titulo = "Libro Test", Autor = "Autor Test", Categoria = "Novela" };
        _mockService.Setup(s => s.ObtenerPorId(1)).Returns(libro);

        var resultado = _controller.Detalle(1) as ViewResult;

        Assert.NotNull(resultado);
        var modelo = Assert.IsType<Libro>(resultado.Model);
        Assert.Equal("Libro Test", modelo.Titulo);
    }

    [Fact]
    public void Detalle_LibroNoExiste_DevuelveNotFound()
    {
        _mockService.Setup(s => s.ObtenerPorId(999)).Returns((Libro?)null);

        var resultado = _controller.Detalle(999);

        Assert.IsType<NotFoundResult>(resultado);
    }

    // ========================================================================
    // CREAR - Registro de libros
    // ========================================================================

    [Fact]
    public void CrearGet_DevuelveVistaConLibroNuevo()
    {
        var resultado = _controller.Crear() as ViewResult;

        Assert.NotNull(resultado);
        Assert.IsType<Libro>(resultado.Model);
    }

    [Fact]
    public void CrearPost_LibroValido_RedirigeAIndex()
    {
        var libro = new Libro
        {
            Titulo = "Nuevo Libro",
            Autor = "Nuevo Autor",
            Categoria = "Novela",
            AnioPublicacion = 2024,
            Disponibles = 3,
            Descripcion = "Descripción de prueba."
        };

        var resultado = _controller.Crear(libro, null) as RedirectToActionResult;

        Assert.NotNull(resultado);
        Assert.Equal("Index", resultado.ActionName);
        _mockService.Verify(s => s.Crear(libro), Times.Once);
    }

    [Fact]
    public void CrearPost_ModeloInvalido_DevuelveVistaConLibro()
    {
        _controller.ModelState.AddModelError("Titulo", "El título es obligatorio.");
        var libro = new Libro();

        var resultado = _controller.Crear(libro, null) as ViewResult;

        Assert.NotNull(resultado);
        Assert.IsType<Libro>(resultado.Model);
        _mockService.Verify(s => s.Crear(It.IsAny<Libro>()), Times.Never);
    }

    [Fact]
    public void CrearPost_ServicioLanzaExcepcion_DevuelveVistaConError()
    {
        var libro = new Libro
        {
            Titulo = "Libro Error",
            Autor = "Autor",
            Categoria = "Novela",
            AnioPublicacion = 2024
        };
        _mockService.Setup(s => s.Crear(It.IsAny<Libro>())).Throws(new Exception("Error de BD"));

        var resultado = _controller.Crear(libro, null) as ViewResult;

        Assert.NotNull(resultado);
        Assert.Equal("Ocurrió un error al guardar el libro. Intente nuevamente.",
            _controller.TempData["Error"]);
    }

    // ========================================================================
    // EDITAR - Actualización de libros
    // ========================================================================

    [Fact]
    public void EditarGet_LibroExiste_DevuelveVistaConLibro()
    {
        var libro = new Libro { Id = 1, Titulo = "Libro Existente" };
        _mockService.Setup(s => s.ObtenerPorId(1)).Returns(libro);

        var resultado = _controller.Editar(1) as ViewResult;

        Assert.NotNull(resultado);
        var modelo = Assert.IsType<Libro>(resultado.Model);
        Assert.Equal("Libro Existente", modelo.Titulo);
    }

    [Fact]
    public void EditarGet_LibroNoExiste_DevuelveNotFound()
    {
        _mockService.Setup(s => s.ObtenerPorId(999)).Returns((Libro?)null);

        var resultado = _controller.Editar(999);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Fact]
    public void EditarPost_LibroValido_RedirigeAIndex()
    {
        var libro = new Libro
        {
            Id = 1,
            Titulo = "Libro Editado",
            Autor = "Autor",
            Categoria = "Novela",
            AnioPublicacion = 2024,
            Disponibles = 2
        };
        _mockService.Setup(s => s.Actualizar(libro)).Returns(true);

        var resultado = _controller.Editar(libro, null) as RedirectToActionResult;

        Assert.NotNull(resultado);
        Assert.Equal("Index", resultado.ActionName);
        _mockService.Verify(s => s.Actualizar(libro), Times.Once);
    }

    [Fact]
    public void EditarPost_LibroNoExisteEnBD_DevuelveNotFound()
    {
        var libro = new Libro { Id = 999, Titulo = "No existe", Autor = "A", Categoria = "C" };
        _mockService.Setup(s => s.Actualizar(libro)).Returns(false);

        var resultado = _controller.Editar(libro, null);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Fact]
    public void EditarPost_ModeloInvalido_DevuelveVistaConLibro()
    {
        _controller.ModelState.AddModelError("Autor", "El autor es obligatorio.");
        var libro = new Libro { Id = 1 };

        var resultado = _controller.Editar(libro, null) as ViewResult;

        Assert.NotNull(resultado);
        _mockService.Verify(s => s.Actualizar(It.IsAny<Libro>()), Times.Never);
    }

    // ========================================================================
    // ELIMINAR - Eliminación de libros
    // ========================================================================

    [Fact]
    public void EliminarGet_LibroExiste_DevuelveVistaConLibro()
    {
        var libro = new Libro { Id = 1, Titulo = "Libro a eliminar" };
        _mockService.Setup(s => s.ObtenerPorId(1)).Returns(libro);

        var resultado = _controller.Eliminar(1) as ViewResult;

        Assert.NotNull(resultado);
        var modelo = Assert.IsType<Libro>(resultado.Model);
        Assert.Equal("Libro a eliminar", modelo.Titulo);
    }

    [Fact]
    public void EliminarGet_LibroNoExiste_DevuelveNotFound()
    {
        _mockService.Setup(s => s.ObtenerPorId(999)).Returns((Libro?)null);

        var resultado = _controller.Eliminar(999);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Fact]
    public void EliminarConfirmado_LibroExiste_EliminaYRedirige()
    {
        _mockService.Setup(s => s.Eliminar(1)).Returns(true);

        var resultado = _controller.EliminarConfirmado(1) as RedirectToActionResult;

        Assert.NotNull(resultado);
        Assert.Equal("Index", resultado.ActionName);
        _mockService.Verify(s => s.Eliminar(1), Times.Once);
    }

    [Fact]
    public void EliminarConfirmado_LibroNoExiste_RedirigeAIndex()
    {
        _mockService.Setup(s => s.Eliminar(999)).Returns(false);

        var resultado = _controller.EliminarConfirmado(999) as RedirectToActionResult;

        Assert.NotNull(resultado);
        Assert.Equal("Index", resultado.ActionName);
    }

    [Fact]
    public void EliminarConfirmado_ServicioLanzaExcepcion_RedirigeConError()
    {
        _mockService.Setup(s => s.Eliminar(1)).Throws(new Exception("Error de BD"));

        var resultado = _controller.EliminarConfirmado(1) as RedirectToActionResult;

        Assert.NotNull(resultado);
        Assert.Equal("Ocurrió un error al eliminar el libro. Intente nuevamente.",
            _controller.TempData["Error"]);
    }

    // ========================================================================
    // ESCENARIOS CON [Theory] - Diferentes datos de entrada
    // ========================================================================

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(100)]
    public void Detalle_ConDiferentesIds_DevuelveLibroCorrecto(int id)
    {
        var libro = new Libro { Id = id, Titulo = $"Libro {id}" };
        _mockService.Setup(s => s.ObtenerPorId(id)).Returns(libro);

        var resultado = _controller.Detalle(id) as ViewResult;

        Assert.NotNull(resultado);
        var modelo = Assert.IsType<Libro>(resultado.Model);
        Assert.Equal(id, modelo.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(99999)]
    public void Detalle_ConIdsInexistentes_DevuelveNotFound(int id)
    {
        _mockService.Setup(s => s.ObtenerPorId(id)).Returns((Libro?)null);

        var resultado = _controller.Detalle(id);

        Assert.IsType<NotFoundResult>(resultado);
    }
}
