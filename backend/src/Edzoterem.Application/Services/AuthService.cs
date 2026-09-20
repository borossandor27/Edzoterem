using Edzoterem.Application.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Application.Security;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Edzoterem.Application.Services;

public class AuthService : IAuthService
{
    private readonly EdzoteremDbContext _db;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthService(EdzoteremDbContext db, IJwtTokenGenerator tokenGenerator)
    {
        _db = db;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var felhasznalo = await _db.Felhasznalok
            .Include(f => f.Dolgozo)
            .Include(f => f.Tag)
            .FirstOrDefaultAsync(f => f.Felhasznalonev == request.Felhasznalonev);

        if (felhasznalo is null || !felhasznalo.Aktiv || !BCrypt.Net.BCrypt.Verify(request.Jelszo, felhasznalo.JelszoHash))
        {
            throw new ApiException("Hibás felhasználónév vagy jelszó.", 401);
        }

        var nev = felhasznalo.Dolgozo is not null
            ? $"{felhasznalo.Dolgozo.Vezeteknev} {felhasznalo.Dolgozo.Keresztnev}"
            : felhasznalo.Tag is not null
                ? $"{felhasznalo.Tag.Vezeteknev} {felhasznalo.Tag.Keresztnev}"
                : felhasznalo.Felhasznalonev;

        felhasznalo.UtolsoBejelentkezes = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        var token = _tokenGenerator.GenerateToken(felhasznalo, nev);

        return new LoginResponse(token, felhasznalo.Szerepkor.ToString(), nev, felhasznalo.JelszoIdeiglenes);
    }

    public async Task<FelhasznaloResponse> GetSajatFelhasznaloAsync(int felhasznaloId)
    {
        var felhasznalo = await _db.Felhasznalok
            .Include(f => f.Dolgozo)
            .Include(f => f.Tag)
            .FirstOrDefaultAsync(f => f.Id == felhasznaloId)
            ?? throw ApiException.NotFound("Felhasználó nem található.");

        var nev = felhasznalo.Dolgozo is not null
            ? $"{felhasznalo.Dolgozo.Vezeteknev} {felhasznalo.Dolgozo.Keresztnev}"
            : felhasznalo.Tag is not null
                ? $"{felhasznalo.Tag.Vezeteknev} {felhasznalo.Tag.Keresztnev}"
                : felhasznalo.Felhasznalonev;

        return new FelhasznaloResponse(felhasznalo.Id, felhasznalo.Felhasznalonev, felhasznalo.Szerepkor.ToString(), nev);
    }

    public async Task JelszoValtoztatasAsync(int felhasznaloId, JelszoValtoztatasRequest request)
    {
        var felhasznalo = await _db.Felhasznalok.FindAsync(felhasznaloId)
            ?? throw ApiException.NotFound("Felhasználó nem található.");

        if (!BCrypt.Net.BCrypt.Verify(request.RegiJelszo, felhasznalo.JelszoHash))
        {
            throw ApiException.BadRequest("A régi jelszó nem megfelelő.");
        }

        felhasznalo.JelszoHash = BCrypt.Net.BCrypt.HashPassword(request.UjJelszo);
        felhasznalo.JelszoIdeiglenes = false;
        await _db.SaveChangesAsync();
    }
}
