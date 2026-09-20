using Edzoterem.Domain.Enums;

namespace Edzoterem.Domain.Entities;

public class EgyeniFoglalkozas
{
    public int Id { get; set; }
    public int SzolgaltatasTipusId { get; set; }
    public SzolgaltatasTipus? SzolgaltatasTipus { get; set; }
    public int DolgozoId { get; set; }
    public Dolgozo? Dolgozo { get; set; }
    public int TagId { get; set; }
    public Tag? Tag { get; set; }
    public DateTime Idopont { get; set; }
    public decimal Ar { get; set; }
    public EgyeniFoglalkozasAllapot Allapot { get; set; } = EgyeniFoglalkozasAllapot.Lefoglalva;
    public int RogzitveAltalId { get; set; }
    public Felhasznalo? RogzitveAltal { get; set; }
    public string? Megjegyzes { get; set; }
}
