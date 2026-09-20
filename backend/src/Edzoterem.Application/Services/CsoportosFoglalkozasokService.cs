using Edzoterem.Application.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Domain.Entities;
using Edzoterem.Domain.Enums;
using Edzoterem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Edzoterem.Application.Services;

public class CsoportosFoglalkozasokService : ICsoportosFoglalkozasokService
{
    private readonly EdzoteremDbContext _db;

    public CsoportosFoglalkozasokService(EdzoteremDbContext db)
    {
        _db = db;
    }

    private static CsoportosFoglalkozasResponse ToResponse(CsoportosFoglalkozas c) => new(
        c.Id, c.Megnevezes, c.EdzoId, c.Edzo?.Dolgozo is not null ? $"{c.Edzo.Dolgozo.Vezeteknev} {c.Edzo.Dolgozo.Keresztnev}" : string.Empty,
        c.Idopont, c.Helyszin, c.MaxLetszam, c.Statusz.ToString(), c.ElutasitasIndoka,
        c.Jelentkezesek.Select(j => new CsoportosJelentkezesResponse(
            j.Id, j.TagId, j.Tag is not null ? $"{j.Tag.Vezeteknev} {j.Tag.Keresztnev}" : string.Empty, j.JelenVolt)).ToList());

    private IQueryable<CsoportosFoglalkozas> QueryWithIncludes() =>
        _db.CsoportosFoglalkozasok
            .Include(c => c.Edzo).ThenInclude(e => e!.Dolgozo)
            .Include(c => c.Jelentkezesek).ThenInclude(j => j.Tag);

    public async Task<List<CsoportosFoglalkozasResponse>> GetAllAsync()
    {
        var lista = await QueryWithIncludes().OrderByDescending(c => c.Idopont).ToListAsync();
        return lista.Select(ToResponse).ToList();
    }

    public async Task<CsoportosFoglalkozasResponse> GetByIdAsync(int id)
    {
        var foglalkozas = await QueryWithIncludes().FirstOrDefaultAsync(c => c.Id == id)
            ?? throw ApiException.NotFound("Csoportos foglalkozás nem található.");
        return ToResponse(foglalkozas);
    }

    public async Task<List<CsoportosFoglalkozasResponse>> GetEdzoSajatjaiAsync(int edzoId)
    {
        var lista = await QueryWithIncludes().Where(c => c.EdzoId == edzoId)
            .OrderByDescending(c => c.Idopont).ToListAsync();
        return lista.Select(ToResponse).ToList();
    }

    public async Task<List<CsoportosFoglalkozasResponse>> GetTagSajatjaiAsync(int tagId)
    {
        var lista = await QueryWithIncludes()
            .Where(c => c.Jelentkezesek.Any(j => j.TagId == tagId))
            .OrderByDescending(c => c.Idopont).ToListAsync();
        return lista.Select(ToResponse).ToList();
    }

    public async Task<CsoportosFoglalkozasResponse> MeghirdetesAsync(int edzoId, int meghirdetteFelhasznaloId, CsoportosFoglalkozasMeghirdetesRequest request)
    {
        if (!await _db.Edzok.AnyAsync(e => e.Id == edzoId))
        {
            throw ApiException.NotFound("Edző nem található.");
        }

        var foglalkozas = new CsoportosFoglalkozas
        {
            Megnevezes = request.Megnevezes,
            EdzoId = edzoId,
            Idopont = request.Idopont,
            Helyszin = request.Helyszin,
            MaxLetszam = request.MaxLetszam,
            Statusz = CsoportosFoglalkozasStatusz.JovahagyasraVar,
            MeghirdetteId = meghirdetteFelhasznaloId
        };

        _db.CsoportosFoglalkozasok.Add(foglalkozas);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(foglalkozas.Id);
    }

    public async Task<CsoportosFoglalkozasResponse> JovahagyasAsync(int id, int tulajdonosFelhasznaloId)
    {
        var foglalkozas = await _db.CsoportosFoglalkozasok.FindAsync(id)
            ?? throw ApiException.NotFound("Csoportos foglalkozás nem található.");

        if (foglalkozas.Statusz != CsoportosFoglalkozasStatusz.JovahagyasraVar)
        {
            throw ApiException.Conflict("Csak jóváhagyásra váró foglalkozás hagyható jóvá.");
        }

        foglalkozas.Statusz = CsoportosFoglalkozasStatusz.Jovahagyva;
        foglalkozas.JovahagytaId = tulajdonosFelhasznaloId;
        foglalkozas.JovahagyasDatuma = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<CsoportosFoglalkozasResponse> ElutasitasAsync(int id, int tulajdonosFelhasznaloId, CsoportosFoglalkozasElutasitasRequest request)
    {
        var foglalkozas = await _db.CsoportosFoglalkozasok.FindAsync(id)
            ?? throw ApiException.NotFound("Csoportos foglalkozás nem található.");

        if (foglalkozas.Statusz != CsoportosFoglalkozasStatusz.JovahagyasraVar)
        {
            throw ApiException.Conflict("Csak jóváhagyásra váró foglalkozás utasítható el.");
        }

        foglalkozas.Statusz = CsoportosFoglalkozasStatusz.Elutasitva;
        foglalkozas.JovahagytaId = tulajdonosFelhasznaloId;
        foglalkozas.JovahagyasDatuma = DateTime.UtcNow;
        foglalkozas.ElutasitasIndoka = request.Indok;
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<CsoportosFoglalkozasResponse> JelentkezesAsync(int id, CsoportosJelentkezesRequest request, int rogzitveAltalId)
    {
        var foglalkozas = await _db.CsoportosFoglalkozasok.Include(c => c.Jelentkezesek)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw ApiException.NotFound("Csoportos foglalkozás nem található.");

        if (foglalkozas.Statusz != CsoportosFoglalkozasStatusz.Jovahagyva)
        {
            throw ApiException.Conflict("Csak jóváhagyott foglalkozásra lehet jelentkeztetni tagot.");
        }

        if (foglalkozas.Jelentkezesek.Any(j => j.TagId == request.TagId))
        {
            throw ApiException.Conflict("A tag már jelentkezett erre a foglalkozásra.");
        }

        if (foglalkozas.Jelentkezesek.Count >= foglalkozas.MaxLetszam)
        {
            throw ApiException.Conflict("A foglalkozás betelt (elérte a maximális létszámot).");
        }

        var tagLetezik = await _db.Tagok.AnyAsync(t => t.Id == request.TagId && t.Aktiv);
        if (!tagLetezik)
        {
            throw ApiException.NotFound("Tag nem található, vagy inaktív.");
        }

        _db.CsoportosJelentkezesek.Add(new CsoportosJelentkezes
        {
            CsoportosFoglalkozasId = id,
            TagId = request.TagId,
            RogzitveAltalId = rogzitveAltalId
        });

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task JelentkezesTorleseAsync(int id, int tagId)
    {
        var jelentkezes = await _db.CsoportosJelentkezesek
            .FirstOrDefaultAsync(j => j.CsoportosFoglalkozasId == id && j.TagId == tagId)
            ?? throw ApiException.NotFound("Jelentkezés nem található.");

        _db.CsoportosJelentkezesek.Remove(jelentkezes);
        await _db.SaveChangesAsync();
    }

    public async Task<CsoportosFoglalkozasResponse> JelenletRogzitesAsync(int id, int edzoId, JelenletRogzitesRequest request)
    {
        var foglalkozas = await _db.CsoportosFoglalkozasok.Include(c => c.Jelentkezesek)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw ApiException.NotFound("Csoportos foglalkozás nem található.");

        if (foglalkozas.EdzoId != edzoId)
        {
            throw new ApiException("Csak a foglalkozást tartó edző rögzítheti a jelenlétet.", 403);
        }

        if (foglalkozas.Statusz != CsoportosFoglalkozasStatusz.Jovahagyva)
        {
            throw ApiException.Conflict("Csak jóváhagyott foglalkozáson rögzíthető jelenlét.");
        }

        foreach (var bejegyzes in request.Jelenletek)
        {
            var jelentkezes = foglalkozas.Jelentkezesek.FirstOrDefault(j => j.TagId == bejegyzes.TagId);
            if (jelentkezes is not null)
            {
                jelentkezes.JelenVolt = bejegyzes.JelenVolt;
            }
        }

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<CsoportosFoglalkozasResponse> LezarasAsync(int id, int edzoId)
    {
        var foglalkozas = await _db.CsoportosFoglalkozasok.FindAsync(id)
            ?? throw ApiException.NotFound("Csoportos foglalkozás nem található.");

        if (foglalkozas.EdzoId != edzoId)
        {
            throw new ApiException("Csak a foglalkozást tartó edző zárhatja le.", 403);
        }

        if (foglalkozas.Statusz != CsoportosFoglalkozasStatusz.Jovahagyva)
        {
            throw ApiException.Conflict("Csak jóváhagyott foglalkozás zárható le.");
        }

        foglalkozas.Statusz = CsoportosFoglalkozasStatusz.Lezarva;
        foglalkozas.LezarasDatuma = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }
}
