using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaWeb.Models;

public class Libro
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(200)]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El autor es obligatorio.")]
    [StringLength(120, ErrorMessage = "El autor no puede superar los 120 caracteres.")]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(80, ErrorMessage = "La categoría no puede superar los 80 caracteres.")]
    [Display(Name = "Categoría")]
    public string Categoria { get; set; } = string.Empty;

    [Range(1450, 2100, ErrorMessage = "Ingrese un año entre 1450 y 2100.")]
    [Display(Name = "Año de publicación")]
    public int AnioPublicacion { get; set; }

    [Range(0, 100000, ErrorMessage = "Ingrese una cantidad entre 0 y 100000.")]
    [Display(Name = "Ejemplares disponibles")]
    public int Disponibles { get; set; }

    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Display(Name = "Imagen")]
    public string ImagenNombre { get; set; } = "sin-imagen.svg";

    // Propiedad calculada: no se almacena en la base de datos, se deriva de Disponibles.
    [NotMapped]
    public bool HayDisponibles => Disponibles > 0;
}
