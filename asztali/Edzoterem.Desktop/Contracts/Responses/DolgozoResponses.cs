namespace Edzoterem.Contracts.Responses;

public record DolgozoResponse(
    int Id,
    string Vezeteknev,
    string Keresztnev,
    string? Becenev,
    string Munkakor,
    string? Telefon,
    string? Email,
    DateOnly SzuletesiDatum,
    string Nem,
    decimal? Oradij,
    bool Aktiv
);

public record MunkarendResponse(int Id, DateOnly Datum, TimeOnly TolIdo, TimeOnly IgIdo, string? Megjegyzes);

public record EdzoResponse(
    int Id,
    int DolgozoId,
    string Nev,
    string? Szakterulet,
    string? Vegzettseg,
    decimal? Oradij
);

public record EdzoElerhetosegResponse(int Id, string HetNapja, TimeOnly TolIdo, TimeOnly IgIdo);
