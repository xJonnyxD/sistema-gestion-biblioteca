namespace BibliotecaWeb.Models;

public class Libro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int AnioPublicacion { get; set; }
    public int Disponibles { get; set; }

    public bool HayDisponibles => Disponibles > 0;
}
