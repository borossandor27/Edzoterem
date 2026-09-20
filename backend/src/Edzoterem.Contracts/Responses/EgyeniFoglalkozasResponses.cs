namespace Edzoterem.Contracts.Responses;

public record SzolgaltatasTipusResponse(int Id, string Nev, decimal AlapertelmezettAr, bool Aktiv);

public record EgyeniFoglalkozasResponse(
    int Id,
    string SzolgaltatasNev,
    int DolgozoId,
    string DolgozoNev,
    int TagId,
    string TagNev,
    DateTime Idopont,
    decimal Ar,
    string Allapot,
    string? Megjegyzes
);
