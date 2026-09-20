namespace Edzoterem.Contracts.Requests;

public record TagRegisztracioRequest(
    string Vezeteknev,
    string Keresztnev,
    DateOnly SzuletesiDatum,
    string? Cim,
    string? Telefon,
    string? Email,
    string? VhKapcsolattartoNev,
    string? VhKapcsolattartoTelefon,
    string Nem,
    string? Megjegyzes
);

public record TagUpdateRequest(
    string Vezeteknev,
    string Keresztnev,
    DateOnly SzuletesiDatum,
    string? Cim,
    string? Telefon,
    string? Email,
    string? VhKapcsolattartoNev,
    string? VhKapcsolattartoTelefon,
    string Nem,
    string? Megjegyzes
);
