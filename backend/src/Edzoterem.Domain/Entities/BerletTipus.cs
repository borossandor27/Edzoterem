using Edzoterem.Domain.Enums;

namespace Edzoterem.Domain.Entities;

public class BerletTipus
{
    public int Id { get; set; }
    public string Nev { get; set; } = string.Empty;
    public BerletKategoria Kategoria { get; set; }
    public decimal Ar { get; set; }
    public int? AlapertelmezettAlkalmak { get; set; }
    public bool Aktiv { get; set; } = true;

    public ICollection<Berlet> Berletek { get; set; } = new List<Berlet>();
}
