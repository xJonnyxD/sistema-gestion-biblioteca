using System.ComponentModel.DataAnnotations;

namespace BibliotecaWeb.Models;

public class Libro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El autor es obligatorio.")]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [Display(Name = "Categoría")]
    public string Categoria { get; set; } = string.Empty;

    [Range(1, 2100, ErrorMessage = "Ingrese un año válido.")]
    [Display(Name = "Año de publicación")]
    public int AnioPublicacion { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser negativa.")]
    [Display(Name = "Ejemplares disponibles")]
    public int Disponibles { get; set; }

    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Display(Name = "Imagen")]
    public string ImagenNombre { get; set; } = "sin-imagen.svg";

    public bool HayDisponibles => Disponibles > 0;
}
