using Edzoterem.Domain.Enums;

namespace Edzoterem.Domain.Entities;

public class Dolgozo
{
    public int Id { get; set; }
    public string Vezeteknev { get; set; } = string.Empty;
    public string Keresztnev { get; set; } = string.Empty;
    public string? Becenev { get; set; }
    public int MunkakorId { get; set; }
    public Munkakor? Munkakor { get; set; }
    public string? SzemelyiSzam { get; set; }
    public string? TajSzam { get; set; }
    public string? Adoszam { get; set; }
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public DateOnly SzuletesiDatum { get; set; }
    public Nem Nem { get; set; }
    public string? FenykepUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public decimal? Oradij { get; set; }
    public string? Megjegyzes { get; set; }
    public bool Aktiv { get; set; } = true;
    public DateTime Letrehozva { get; set; } = DateTime.UtcNow;
    public DateTime? Modositva { get; set; }

    public Edzo? Edzo { get; set; }
    public ICollection<Munkarend> Munkarendek { get; set; } = new List<Munkarend>();
    public Felhasznalo? Felhasznalo { get; set; }
}
