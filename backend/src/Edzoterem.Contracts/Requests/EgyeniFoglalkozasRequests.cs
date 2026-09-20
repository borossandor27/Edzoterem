namespace Edzoterem.Contracts.Requests;

public record SzolgaltatasTipusUpsertRequest(string Nev, decimal AlapertelmezettAr, bool Aktiv);

public record EgyeniFoglalkozasRequest(int SzolgaltatasTipusId, int DolgozoId, int TagId, DateTime Idopont, decimal? ArFelulir, string? Megjegyzes);

public record EgyeniFoglalkozasStatuszRequest(string Allapot);
