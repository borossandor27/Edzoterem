namespace Edzoterem.Contracts.Responses;

public record BerletTipusResponse(int Id, string Nev, string Kategoria, decimal Ar, int? AlapertelmezettAlkalmak, bool Aktiv);

public record BerletResponse(
    int Id,
    int TagId,
    string TagNev,
    string BerletTipusNev,
    DateOnly KezdoDatum,
    DateOnly LejaratiDatum,
    decimal ArFizetve,
    int? HatralevoAlkalmak,
    string Statusz,
    bool Lejaro
);
