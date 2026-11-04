using BibliotecaWeb.Models;

namespace BibliotecaWeb.Tests;

public class LibroModelTests
{
    [Fact]
    public void HayDisponibles_ConStock_DevuelveTrue()
    {
        var libro = new Libro { Disponibles = 5 };

        Assert.True(libro.HayDisponibles);
    }

    [Fact]
    public void HayDisponibles_SinStock_DevuelveFalse()
    {
        var libro = new Libro { Disponibles = 0 };

        Assert.False(libro.HayDisponibles);
    }

    [Fact]
    public void ImagenNombre_PorDefecto_EsSinImagen()
    {
        var libro = new Libro();

        Assert.Equal("sin-imagen.svg", libro.ImagenNombre);
    }

    [Fact]
    public void Titulo_PorDefecto_EsCadenaVacia()
    {
        var libro = new Libro();

        Assert.Equal(string.Empty, libro.Titulo);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99999)]
    public void HayDisponibles_ConDiferentesCantidades_DevuelveTrue(int cantidad)
    {
        var libro = new Libro { Disponibles = cantidad };

        Assert.True(libro.HayDisponibles);
    }
}
