namespace Edzoterem.Contracts.Responses;

public record CsoportosJelentkezesResponse(int Id, int TagId, string TagNev, bool? JelenVolt);

public record CsoportosFoglalkozasResponse(
    int Id,
    string Megnevezes,
    int EdzoId,
    string EdzoNev,
    DateTime Idopont,
    string Helyszin,
    int MaxLetszam,
    string Statusz,
    string? ElutasitasIndoka,
    List<CsoportosJelentkezesResponse> Jelentkezesek
);
