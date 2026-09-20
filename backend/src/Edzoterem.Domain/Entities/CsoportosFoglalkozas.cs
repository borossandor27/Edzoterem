using Edzoterem.Domain.Enums;

namespace Edzoterem.Domain.Entities;

public class CsoportosFoglalkozas
{
    public int Id { get; set; }
    public string Megnevezes { get; set; } = string.Empty;
    public int EdzoId { get; set; }
    public Edzo? Edzo { get; set; }
    public DateTime Idopont { get; set; }
    public string Helyszin { get; set; } = string.Empty;
    public int MaxLetszam { get; set; }
    public CsoportosFoglalkozasStatusz Statusz { get; set; } = CsoportosFoglalkozasStatusz.JovahagyasraVar;

    public int MeghirdetteId { get; set; }
    public Felhasznalo? Meghirdette { get; set; }
    public int? JovahagytaId { get; set; }
    public Felhasznalo? Jovahagyta { get; set; }
    public DateTime? JovahagyasDatuma { get; set; }
    public DateTime? LezarasDatuma { get; set; }
    public string? ElutasitasIndoka { get; set; }

    public ICollection<CsoportosJelentkezes> Jelentkezesek { get; set; } = new List<CsoportosJelentkezes>();
}
