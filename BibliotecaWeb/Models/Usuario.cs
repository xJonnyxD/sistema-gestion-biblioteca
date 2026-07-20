namespace BibliotecaWeb.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Carne { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
