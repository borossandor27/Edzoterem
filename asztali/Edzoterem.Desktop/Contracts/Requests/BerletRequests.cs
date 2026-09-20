namespace Edzoterem.Contracts.Requests;

public record BerletTipusUpsertRequest(string Nev, string Kategoria, decimal Ar, int? AlapertelmezettAlkalmak, bool Aktiv);

public record BerletHozzarendelesRequest(int TagId, int BerletTipusId, DateOnly KezdoDatum, DateOnly LejaratiDatum, decimal? ArFelulir, int? AlkalmakFelulir);
