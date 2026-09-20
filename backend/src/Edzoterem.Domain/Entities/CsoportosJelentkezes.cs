namespace Edzoterem.Domain.Entities;

public class CsoportosJelentkezes
{
    public int Id { get; set; }
    public int CsoportosFoglalkozasId { get; set; }
    public CsoportosFoglalkozas? CsoportosFoglalkozas { get; set; }
    public int TagId { get; set; }
    public Tag? Tag { get; set; }
    public DateTime JelentkezesDatuma { get; set; } = DateTime.UtcNow;
    public int RogzitveAltalId { get; set; }
    public Felhasznalo? RogzitveAltal { get; set; }
    public bool? JelenVolt { get; set; }
}
