namespace Edzoterem.Contracts.Requests;

public record CsoportosFoglalkozasMeghirdetesRequest(string Megnevezes, DateTime Idopont, string Helyszin, int MaxLetszam);

public record CsoportosFoglalkozasElutasitasRequest(string Indok);

public record CsoportosJelentkezesRequest(int TagId);

public record JelenletBejegyzes(int TagId, bool JelenVolt);

public record JelenletRogzitesRequest(List<JelenletBejegyzes> Jelenletek);
