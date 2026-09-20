using Edzoterem.Domain.Enums;

namespace Edzoterem.Domain.Entities;

public class Berlet
{
    public int Id { get; set; }
    public int TagId { get; set; }
    public Tag? Tag { get; set; }
    public int BerletTipusId { get; set; }
    public BerletTipus? BerletTipus { get; set; }
    public DateOnly KezdoDatum { get; set; }
    public DateOnly LejaratiDatum { get; set; }
    public decimal ArFizetve { get; set; }
    public int? HatralevoAlkalmak { get; set; }
    public BerletStatusz Statusz { get; set; } = BerletStatusz.Aktiv;
    public int LetrehozvaAltalId { get; set; }
    public Felhasznalo? LetrehozvaAltal { get; set; }
}
