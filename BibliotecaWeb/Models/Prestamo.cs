namespace BibliotecaWeb.Models;

public enum EstadoPrestamo
{
    Pendiente,
    Devuelto,
    Atrasado
}

public class Prestamo
{
    public int Id { get; set; }
    public string Libro { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public EstadoPrestamo Estado { get; set; }
}
