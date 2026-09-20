namespace Edzoterem.Domain.Entities;

public class Edzo
{
    public int Id { get; set; }
    public int DolgozoId { get; set; }
    public Dolgozo? Dolgozo { get; set; }
    public string? Szakterulet { get; set; }
    public string? Vegzettseg { get; set; }
    public decimal? Oradij { get; set; }

    public ICollection<EdzoElerhetoseg> Elerhetosegek { get; set; } = new List<EdzoElerhetoseg>();
    public ICollection<CsoportosFoglalkozas> CsoportosFoglalkozasok { get; set; } = new List<CsoportosFoglalkozas>();
}
