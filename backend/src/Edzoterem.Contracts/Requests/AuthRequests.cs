namespace Edzoterem.Contracts.Requests;

public record LoginRequest(string Felhasznalonev, string Jelszo);

public record JelszoValtoztatasRequest(string RegiJelszo, string UjJelszo);
