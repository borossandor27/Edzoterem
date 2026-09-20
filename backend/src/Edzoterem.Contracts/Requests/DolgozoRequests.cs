namespace Edzoterem.Contracts.Requests;

public record DolgozoUpsertRequest(
    string Vezeteknev,
    string Keresztnev,
    string? Becenev,
    int MunkakorId,
    string? SzemelyiSzam,
    string? TajSzam,
    string? Adoszam,
    string? Telefon,
    string? Email,
    DateOnly SzuletesiDatum,
    string Nem,
    string? FenykepUrl,
    string? PortfolioUrl,
    decimal? Oradij,
    string? Megjegyzes
);

public record MunkarendRequest(DateOnly Datum, TimeOnly TolIdo, TimeOnly IgIdo, string? Megjegyzes);

public record EdzoUpsertRequest(string? Szakterulet, string? Vegzettseg, decimal? Oradij);

public record EdzoElerhetosegRequest(string HetNapja, TimeOnly TolIdo, TimeOnly IgIdo);
