namespace Edzoterem.Domain.Entities;

public class Munkarend
{
    public int Id { get; set; }
    public int DolgozoId { get; set; }
    public Dolgozo? Dolgozo { get; set; }
    public DateOnly Datum { get; set; }
    public TimeOnly TolIdo { get; set; }
    public TimeOnly IgIdo { get; set; }
    public string? Megjegyzes { get; set; }
}
