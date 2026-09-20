namespace Edzoterem.Contracts.Responses;

public record LoginResponse(string Token, string Szerepkor, string Nev, bool JelszoIdeiglenes);

public record FelhasznaloResponse(int Id, string Felhasznalonev, string Szerepkor, string Nev);
