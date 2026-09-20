namespace Edzoterem.Contracts.Responses;

public record TagResponse(
    int Id,
    string Vezeteknev,
    string Keresztnev,
    DateOnly SzuletesiDatum,
    string? Cim,
    string? Telefon,
    string? Email,
    DateOnly RegisztracioDatuma,
    string? VhKapcsolattartoNev,
    string? VhKapcsolattartoTelefon,
    string Nem,
    string? Megjegyzes,
    bool Aktiv
);

public record TagRegisztracioValaszResponse(TagResponse Tag, string Felhasznalonev, string IdeiglenesJelszo);
