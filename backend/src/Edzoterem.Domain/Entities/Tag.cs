using Edzoterem.Domain.Enums;

namespace Edzoterem.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Vezeteknev { get; set; } = string.Empty;
    public string Keresztnev { get; set; } = string.Empty;
    public DateOnly SzuletesiDatum { get; set; }
    public string? Cim { get; set; }
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public DateOnly RegisztracioDatuma { get; set; }
    public string? VhKapcsolattartoNev { get; set; }
    public string? VhKapcsolattartoTelefon { get; set; }
    public Nem Nem { get; set; }
    public string? Megjegyzes { get; set; }
    public bool Aktiv { get; set; } = true;

    public Felhasznalo? Felhasznalo { get; set; }
    public ICollection<Berlet> Berletek { get; set; } = new List<Berlet>();
    public ICollection<CsoportosJelentkezes> CsoportosJelentkezesek { get; set; } = new List<CsoportosJelentkezes>();
    public ICollection<EgyeniFoglalkozas> EgyeniFoglalkozasok { get; set; } = new List<EgyeniFoglalkozas>();
}
