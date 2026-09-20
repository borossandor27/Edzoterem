namespace Edzoterem.Domain.Entities;

public class Munkakor
{
    public int Id { get; set; }
    public string Nev { get; set; } = string.Empty;

    public ICollection<Dolgozo> Dolgozok { get; set; } = new List<Dolgozo>();
}
