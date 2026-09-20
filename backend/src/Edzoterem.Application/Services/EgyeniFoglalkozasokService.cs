using Edzoterem.Application.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Domain.Entities;
using Edzoterem.Domain.Enums;
using Edzoterem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Edzoterem.Application.Services;

public class EgyeniFoglalkozasokService : IEgyeniFoglalkozasokService
{
    private readonly EdzoteremDbContext _db;

    public EgyeniFoglalkozasokService(EdzoteremDbContext db)
    {
        _db = db;
    }

    private static SzolgaltatasTipusResponse ToResponse(SzolgaltatasTipus s) =>
        new(s.Id, s.Nev, s.AlapertelmezettAr, s.Aktiv);

    private static EgyeniFoglalkozasResponse ToResponse(EgyeniFoglalkozas f) => new(
        f.Id, f.SzolgaltatasTipus?.Nev ?? string.Empty, f.DolgozoId,
        f.Dolgozo is not null ? $"{f.Dolgozo.Vezeteknev} {f.Dolgozo.Keresztnev}" : string.Empty,
        f.TagId, f.Tag is not null ? $"{f.Tag.Vezeteknev} {f.Tag.Keresztnev}" : string.Empty,
        f.Idopont, f.Ar, f.Allapot.ToString(), f.Megjegyzes);

    private IQueryable<EgyeniFoglalkozas> QueryWithIncludes() =>
        _db.EgyeniFoglalkozasok.Include(f => f.SzolgaltatasTipus).Include(f => f.Dolgozo).Include(f => f.Tag);

    public async Task<List<SzolgaltatasTipusResponse>> GetSzolgaltatasTipusokAsync()
    {
        var tipusok = await _db.SzolgaltatasTipusok.ToListAsync();
        return tipusok.Select(ToResponse).ToList();
    }

    public async Task<SzolgaltatasTipusResponse> CreateSzolgaltatasTipusAsync(SzolgaltatasTipusUpsertRequest request)
    {
        var tipus = new SzolgaltatasTipus
        {
            Nev = request.Nev,
            AlapertelmezettAr = request.AlapertelmezettAr,
            Aktiv = request.Aktiv
        };

        _db.SzolgaltatasTipusok.Add(tipus);
        await _db.SaveChangesAsync();
        return ToResponse(tipus);
    }

    public async Task<EgyeniFoglalkozasResponse> RogzitesAsync(EgyeniFoglalkozasRequest request, int rogzitveAltalId)
    {
        var tipus = await _db.SzolgaltatasTipusok.FindAsync(request.SzolgaltatasTipusId)
            ?? throw ApiException.NotFound("Szolgáltatástípus nem található.");

        if (!await _db.Dolgozok.AnyAsync(d => d.Id == request.DolgozoId && d.Aktiv))
        {
            throw ApiException.NotFound("Szolgáltatást nyújtó dolgozó nem található, vagy inaktív.");
        }

        if (!await _db.Tagok.AnyAsync(t => t.Id == request.TagId && t.Aktiv))
        {
            throw ApiException.NotFound("Tag nem található, vagy inaktív.");
        }

        var foglalkozas = new EgyeniFoglalkozas
        {
            SzolgaltatasTipusId = request.SzolgaltatasTipusId,
            DolgozoId = request.DolgozoId,
            TagId = request.TagId,
            Idopont = request.Idopont,
            Ar = request.ArFelulir ?? tipus.AlapertelmezettAr,
            Allapot = EgyeniFoglalkozasAllapot.Lefoglalva,
            RogzitveAltalId = rogzitveAltalId,
            Megjegyzes = request.Megjegyzes
        };

        _db.EgyeniFoglalkozasok.Add(foglalkozas);
        await _db.SaveChangesAsync();

        var mentett = await QueryWithIncludes().FirstAsync(f => f.Id == foglalkozas.Id);
        return ToResponse(mentett);
    }

    public async Task<EgyeniFoglalkozasResponse> StatuszFrissitesAsync(int id, EgyeniFoglalkozasStatuszRequest request)
    {
        var foglalkozas = await QueryWithIncludes().FirstOrDefaultAsync(f => f.Id == id)
            ?? throw ApiException.NotFound("Egyéni foglalkozás nem található.");

        var ujAllapot = Enum.Parse<EgyeniFoglalkozasAllapot>(request.Allapot, ignoreCase: true);
        if (foglalkozas.Allapot != EgyeniFoglalkozasAllapot.Lefoglalva)
        {
            throw ApiException.Conflict("Csak lefoglalt állapotú foglalkozás állapota módosítható.");
        }

        foglalkozas.Allapot = ujAllapot;
        await _db.SaveChangesAsync();
        return ToResponse(foglalkozas);
    }

    public async Task<List<EgyeniFoglalkozasResponse>> GetTagSajatjaiAsync(int tagId)
    {
        var lista = await QueryWithIncludes().Where(f => f.TagId == tagId)
            .OrderByDescending(f => f.Idopont).ToListAsync();
        return lista.Select(ToResponse).ToList();
    }

    public async Task<List<EgyeniFoglalkozasResponse>> GetDolgozoSajatjaiAsync(int dolgozoId)
    {
        var lista = await QueryWithIncludes().Where(f => f.DolgozoId == dolgozoId)
            .OrderByDescending(f => f.Idopont).ToListAsync();
        return lista.Select(ToResponse).ToList();
    }
}
