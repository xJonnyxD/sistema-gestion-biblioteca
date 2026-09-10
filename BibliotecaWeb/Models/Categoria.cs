using System.ComponentModel.DataAnnotations;

namespace BibliotecaWeb.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    public bool Activa { get; set; }
}
