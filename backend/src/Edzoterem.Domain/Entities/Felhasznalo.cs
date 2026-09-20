using Edzoterem.Domain.Enums;

namespace Edzoterem.Domain.Entities;

public class Felhasznalo
{
    public int Id { get; set; }
    public string Felhasznalonev { get; set; } = string.Empty;
    public string JelszoHash { get; set; } = string.Empty;
    public Szerepkor Szerepkor { get; set; }
    public int? DolgozoId { get; set; }
    public Dolgozo? Dolgozo { get; set; }
    public int? TagId { get; set; }
    public Tag? Tag { get; set; }
    public bool Aktiv { get; set; } = true;
    public bool JelszoIdeiglenes { get; set; }
    public DateTime? UtolsoBejelentkezes { get; set; }
}
