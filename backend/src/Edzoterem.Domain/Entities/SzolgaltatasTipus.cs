namespace Edzoterem.Domain.Entities;

public class SzolgaltatasTipus
{
    public int Id { get; set; }
    public string Nev { get; set; } = string.Empty;
    public decimal AlapertelmezettAr { get; set; }
    public bool Aktiv { get; set; } = true;

    public ICollection<EgyeniFoglalkozas> EgyeniFoglalkozasok { get; set; } = new List<EgyeniFoglalkozas>();
}
