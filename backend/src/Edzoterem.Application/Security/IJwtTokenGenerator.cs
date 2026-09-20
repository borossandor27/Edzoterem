using Edzoterem.Domain.Entities;

namespace Edzoterem.Application.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(Felhasznalo felhasznalo, string nev);
}
