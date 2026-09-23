using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers;

public class LibrosController : Controller
{
    private const string ImagenPorDefecto = "sin-imagen.svg";
    private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _entorno;

    // El controlador recibe el DbContext por inyección de dependencias. A través de él
    // realiza todas las operaciones contra la base de datos usando Entity Framework Core.
    public LibrosController(ApplicationDbContext context, IWebHostEnvironment entorno)
    {
        _context = context;
        _entorno = entorno;
    }

    // MOSTRAR: consulta los libros almacenados en la base de datos. Si se recibe un
    // término de búsqueda, filtra por título, autor o categoría directamente en la
    // consulta (Entity Framework Core lo traduce a un WHERE ... LIKE en SQL Server).
    public IActionResult Index(string? buscar)
    {
        var consulta = _context.Libros.AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var termino = buscar.Trim();
            consulta = consulta.Where(l =>
                l.Titulo.Contains(termino) ||
                l.Autor.Contains(termino) ||
                l.Categoria.Contains(termino));
        }

        var libros = consulta
            .OrderBy(l => l.Titulo)
            .ToList();

        ViewData["Buscar"] = buscar;
        return View(libros);
    }

    public IActionResult Detalle(int id)
    {
        // Find() localiza el libro por su clave primaria.
        var libro = _context.Libros.Find(id);
        if (libro is null)
        {
            return NotFound();
        }

        return View(libro);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Libro());
    }

    // AGREGAR: registra un nuevo libro en la base de datos mediante Entity Framework Core.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Libro libro, IFormFile? imagen)
    {
        if (!ModelState.IsValid)
        {
            return View(libro);
        }

        libro.ImagenNombre = GuardarImagen(imagen) ?? ImagenPorDefecto;

        // Add() marca la nueva entidad para inserción y SaveChanges() confirma
        // los cambios, ejecutando el INSERT correspondiente en SQL Server.
        _context.Libros.Add(libro);
        _context.SaveChanges();

        TempData["Exito"] = $"El libro «{libro.Titulo}» se agregó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        // Find() localiza el libro a editar y se envía al formulario.
        var libro = _context.Libros.Find(id);
        if (libro is null)
        {
            return NotFound();
        }

        return View(libro);
    }

    // EDITAR: modifica un libro existente mediante Entity Framework Core.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Libro libro, IFormFile? imagen)
    {
        if (!ModelState.IsValid)
        {
            return View(libro);
        }

        // Find(): localiza el registro existente en la base de datos por su Id.
        var libroActual = _context.Libros.Find(libro.Id);
        if (libroActual is null)
        {
            return NotFound();
        }

        // Se actualizan los campos con los valores enviados desde el formulario.
        libroActual.Titulo = libro.Titulo;
        libroActual.Autor = libro.Autor;
        libroActual.Categoria = libro.Categoria;
        libroActual.AnioPublicacion = libro.AnioPublicacion;
        libroActual.Disponibles = libro.Disponibles;
        libroActual.Descripcion = libro.Descripcion;

        // Si se subió una nueva portada se reemplaza; si no, se conserva la actual.
        var nuevaImagen = GuardarImagen(imagen);
        if (nuevaImagen is not null)
        {
            libroActual.ImagenNombre = nuevaImagen;
        }

        // Update() marca la entidad como modificada y SaveChanges() guarda el
        // UPDATE correspondiente en SQL Server.
        _context.Libros.Update(libroActual);
        _context.SaveChanges();

        TempData["Exito"] = $"El libro «{libroActual.Titulo}» se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        // Find() localiza el libro a eliminar para mostrar la confirmación.
        var libro = _context.Libros.Find(id);
        if (libro is null)
        {
            return NotFound();
        }

        return View(libro);
    }

    // ELIMINAR: quita un libro de la base de datos mediante Entity Framework Core.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarConfirmado(int id)
    {
        // Find() localiza el registro, Remove() lo marca para borrado y
        // SaveChanges() ejecuta el DELETE en SQL Server.
        var libro = _context.Libros.Find(id);
        if (libro is not null)
        {
            _context.Libros.Remove(libro);
            _context.SaveChanges();
            TempData["Exito"] = $"El libro «{libro.Titulo}» se eliminó correctamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Guarda la imagen recibida en wwwroot/images con un nombre único y devuelve
    /// ese nombre. Devuelve null si no se envió ningún archivo o si la extensión
    /// no está permitida, para que el llamador conserve la imagen previa.
    /// </summary>
    private string? GuardarImagen(IFormFile? imagen)
    {
        if (imagen is null || imagen.Length == 0)
        {
            return null;
        }

        var extension = Path.GetExtension(imagen.FileName).ToLowerInvariant();
        if (!ExtensionesPermitidas.Contains(extension))
        {
            return null;
        }

        var carpeta = Path.Combine(_entorno.WebRootPath, "images");
        Directory.CreateDirectory(carpeta);

        var nombreArchivo = $"{Guid.NewGuid():N}{extension}";
        var ruta = Path.Combine(carpeta, nombreArchivo);

        using var stream = new FileStream(ruta, FileMode.Create);
        imagen.CopyTo(stream);

        return nombreArchivo;
    }
}
