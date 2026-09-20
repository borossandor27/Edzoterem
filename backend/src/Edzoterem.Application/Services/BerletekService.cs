using Edzoterem.Application.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Domain.Entities;
using Edzoterem.Domain.Enums;
using Edzoterem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Edzoterem.Application.Services;

public class BerletekService : IBerletekService
{
    private readonly EdzoteremDbContext _db;
    private const int LejaroKuszobNapokban = 7;

    public BerletekService(EdzoteremDbContext db)
    {
        _db = db;
    }

    private static BerletTipusResponse ToResponse(BerletTipus bt) =>
        new(bt.Id, bt.Nev, bt.Kategoria.ToString(), bt.Ar, bt.AlapertelmezettAlkalmak, bt.Aktiv);

    private static BerletResponse ToResponse(Berlet b)
    {
        var maiNap = DateOnly.FromDateTime(DateTime.UtcNow);
        var lejaro = b.LejaratiDatum <= maiNap.AddDays(LejaroKuszobNapokban);
        return new BerletResponse(
            b.Id, b.TagId, b.Tag is not null ? $"{b.Tag.Vezeteknev} {b.Tag.Keresztnev}" : string.Empty,
            b.BerletTipus?.Nev ?? string.Empty, b.KezdoDatum, b.LejaratiDatum, b.ArFizetve,
            b.HatralevoAlkalmak, b.Statusz.ToString(), lejaro);
    }

    public async Task<List<BerletTipusResponse>> GetTipusokAsync()
    {
        var tipusok = await _db.BerletTipusok.ToListAsync();
        return tipusok.Select(ToResponse).ToList();
    }

    public async Task<BerletTipusResponse> CreateTipusAsync(BerletTipusUpsertRequest request)
    {
        var tipus = new BerletTipus
        {
            Nev = request.Nev,
            Kategoria = Enum.Parse<BerletKategoria>(request.Kategoria, ignoreCase: true),
            Ar = request.Ar,
            AlapertelmezettAlkalmak = request.AlapertelmezettAlkalmak,
            Aktiv = request.Aktiv
        };

        _db.BerletTipusok.Add(tipus);
        await _db.SaveChangesAsync();
        return ToResponse(tipus);
    }

    public async Task<BerletTipusResponse> UpdateTipusAsync(int id, BerletTipusUpsertRequest request)
    {
        var tipus = await _db.BerletTipusok.FindAsync(id) ?? throw ApiException.NotFound("Bérlettípus nem található.");

        tipus.Nev = request.Nev;
        tipus.Kategoria = Enum.Parse<BerletKategoria>(request.Kategoria, ignoreCase: true);
        tipus.Ar = request.Ar;
        tipus.AlapertelmezettAlkalmak = request.AlapertelmezettAlkalmak;
        tipus.Aktiv = request.Aktiv;

        await _db.SaveChangesAsync();
        return ToResponse(tipus);
    }

    public async Task<BerletResponse> HozzarendelesAsync(BerletHozzarendelesRequest request, int letrehozvaAltalId)
    {
        var tagLetezik = await _db.Tagok.AnyAsync(t => t.Id == request.TagId && t.Aktiv);
        if (!tagLetezik)
        {
            throw ApiException.NotFound("Tag nem található, vagy inaktív.");
        }

        var tipus = await _db.BerletTipusok.FindAsync(request.BerletTipusId)
            ?? throw ApiException.NotFound("Bérlettípus nem található.");

        var berlet = new Berlet
        {
            TagId = request.TagId,
            BerletTipusId = request.BerletTipusId,
            KezdoDatum = request.KezdoDatum,
            LejaratiDatum = request.LejaratiDatum,
            ArFizetve = request.ArFelulir ?? tipus.Ar,
            HatralevoAlkalmak = request.AlkalmakFelulir ?? tipus.AlapertelmezettAlkalmak,
            Statusz = BerletStatusz.Aktiv,
            LetrehozvaAltalId = letrehozvaAltalId
        };

        _db.Berletek.Add(berlet);
        await _db.SaveChangesAsync();

        await _db.Entry(berlet).Reference(b => b.Tag).LoadAsync();
        await _db.Entry(berlet).Reference(b => b.BerletTipus).LoadAsync();
        return ToResponse(berlet);
    }

    public async Task<List<BerletResponse>> GetTagBerleteiAsync(int tagId)
    {
        var berletek = await _db.Berletek.Include(b => b.Tag).Include(b => b.BerletTipus)
            .Where(b => b.TagId == tagId)
            .OrderByDescending(b => b.KezdoDatum)
            .ToListAsync();
        return berletek.Select(ToResponse).ToList();
    }

    public async Task<List<BerletResponse>> GetLejarokAsync()
    {
        var kuszob = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(LejaroKuszobNapokban);
        var berletek = await _db.Berletek.Include(b => b.Tag).Include(b => b.BerletTipus)
            .Where(b => b.Statusz == BerletStatusz.Aktiv && b.LejaratiDatum <= kuszob)
            .OrderBy(b => b.LejaratiDatum)
            .ToListAsync();
        return berletek.Select(ToResponse).ToList();
    }
}
