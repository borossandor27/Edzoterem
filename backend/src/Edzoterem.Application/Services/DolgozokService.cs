using Edzoterem.Application.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Domain.Entities;
using Edzoterem.Domain.Enums;
using Edzoterem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Edzoterem.Application.Services;

public class DolgozokService : IDolgozokService
{
    private readonly EdzoteremDbContext _db;

    public DolgozokService(EdzoteremDbContext db)
    {
        _db = db;
    }

    private static DolgozoResponse ToResponse(Dolgozo d) => new(
        d.Id, d.Vezeteknev, d.Keresztnev, d.Becenev, d.Munkakor?.Nev ?? string.Empty,
        d.Telefon, d.Email, d.SzuletesiDatum, d.Nem.ToString(), d.Oradij, d.Aktiv);

    private static EdzoResponse ToResponse(Edzo e) => new(
        e.Id, e.DolgozoId, e.Dolgozo is not null ? $"{e.Dolgozo.Vezeteknev} {e.Dolgozo.Keresztnev}" : string.Empty,
        e.Szakterulet, e.Vegzettseg, e.Oradij);

    public async Task<List<DolgozoResponse>> GetAllAsync()
    {
        var dolgozok = await _db.Dolgozok.Include(d => d.Munkakor).ToListAsync();
        return dolgozok.Select(ToResponse).ToList();
    }

    public async Task<DolgozoResponse> GetByIdAsync(int id)
    {
        var dolgozo = await _db.Dolgozok.Include(d => d.Munkakor).FirstOrDefaultAsync(d => d.Id == id)
            ?? throw ApiException.NotFound("Dolgozó nem található.");
        return ToResponse(dolgozo);
    }

    public async Task<DolgozoResponse> CreateAsync(DolgozoUpsertRequest request)
    {
        var dolgozo = new Dolgozo
        {
            Vezeteknev = request.Vezeteknev,
            Keresztnev = request.Keresztnev,
            Becenev = request.Becenev,
            MunkakorId = request.MunkakorId,
            SzemelyiSzam = request.SzemelyiSzam,
            TajSzam = request.TajSzam,
            Adoszam = request.Adoszam,
            Telefon = request.Telefon,
            Email = request.Email,
            SzuletesiDatum = request.SzuletesiDatum,
            Nem = Enum.Parse<Nem>(request.Nem, ignoreCase: true),
            FenykepUrl = request.FenykepUrl,
            PortfolioUrl = request.PortfolioUrl,
            Oradij = request.Oradij,
            Megjegyzes = request.Megjegyzes
        };

        _db.Dolgozok.Add(dolgozo);
        await _db.SaveChangesAsync();
        await _db.Entry(dolgozo).Reference(d => d.Munkakor).LoadAsync();
        return ToResponse(dolgozo);
    }

    public async Task<DolgozoResponse> UpdateAsync(int id, DolgozoUpsertRequest request)
    {
        var dolgozo = await _db.Dolgozok.Include(d => d.Munkakor).FirstOrDefaultAsync(d => d.Id == id)
            ?? throw ApiException.NotFound("Dolgozó nem található.");

        dolgozo.Vezeteknev = request.Vezeteknev;
        dolgozo.Keresztnev = request.Keresztnev;
        dolgozo.Becenev = request.Becenev;
        dolgozo.MunkakorId = request.MunkakorId;
        dolgozo.SzemelyiSzam = request.SzemelyiSzam;
        dolgozo.TajSzam = request.TajSzam;
        dolgozo.Adoszam = request.Adoszam;
        dolgozo.Telefon = request.Telefon;
        dolgozo.Email = request.Email;
        dolgozo.SzuletesiDatum = request.SzuletesiDatum;
        dolgozo.Nem = Enum.Parse<Nem>(request.Nem, ignoreCase: true);
        dolgozo.FenykepUrl = request.FenykepUrl;
        dolgozo.PortfolioUrl = request.PortfolioUrl;
        dolgozo.Oradij = request.Oradij;
        dolgozo.Megjegyzes = request.Megjegyzes;
        dolgozo.Modositva = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return ToResponse(dolgozo);
    }

    public async Task KileptetesAsync(int id)
    {
        var dolgozo = await _db.Dolgozok.FindAsync(id)
            ?? throw ApiException.NotFound("Dolgozó nem található.");
        dolgozo.Aktiv = false;
        dolgozo.Modositva = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<List<MunkarendResponse>> GetMunkarendAsync(int dolgozoId)
    {
        var munkarendek = await _db.Munkarendek.Where(m => m.DolgozoId == dolgozoId)
            .OrderBy(m => m.Datum)
            .ToListAsync();
        return munkarendek
            .Select(m => new MunkarendResponse(m.Id, m.Datum, m.TolIdo, m.IgIdo, m.Megjegyzes))
            .ToList();
    }

    public async Task<MunkarendResponse> AddMunkarendAsync(int dolgozoId, MunkarendRequest request)
    {
        if (!await _db.Dolgozok.AnyAsync(d => d.Id == dolgozoId))
        {
            throw ApiException.NotFound("Dolgozó nem található.");
        }

        var munkarend = new Munkarend
        {
            DolgozoId = dolgozoId,
            Datum = request.Datum,
            TolIdo = request.TolIdo,
            IgIdo = request.IgIdo,
            Megjegyzes = request.Megjegyzes
        };

        _db.Munkarendek.Add(munkarend);
        await _db.SaveChangesAsync();
        return new MunkarendResponse(munkarend.Id, munkarend.Datum, munkarend.TolIdo, munkarend.IgIdo, munkarend.Megjegyzes);
    }

    public async Task<List<EdzoResponse>> GetEdzokAsync()
    {
        var edzok = await _db.Edzok.Include(e => e.Dolgozo)
            .Where(e => e.Dolgozo!.Aktiv)
            .ToListAsync();
        return edzok.Select(ToResponse).ToList();
    }

    public async Task<int> GetEdzoIdByDolgozoIdAsync(int dolgozoId)
    {
        var edzo = await _db.Edzok.FirstOrDefaultAsync(e => e.DolgozoId == dolgozoId)
            ?? throw ApiException.NotFound("A bejelentkezett felhasználóhoz nem tartozik edző rekord.");
        return edzo.Id;
    }

    public async Task<EdzoResponse> CreateEdzoAsync(int dolgozoId, EdzoUpsertRequest request)
    {
        if (!await _db.Dolgozok.AnyAsync(d => d.Id == dolgozoId))
        {
            throw ApiException.NotFound("Dolgozó nem található.");
        }

        if (await _db.Edzok.AnyAsync(e => e.DolgozoId == dolgozoId))
        {
            throw ApiException.Conflict("Ehhez a dolgozóhoz már tartozik edző rekord.");
        }

        var edzo = new Edzo
        {
            DolgozoId = dolgozoId,
            Szakterulet = request.Szakterulet,
            Vegzettseg = request.Vegzettseg,
            Oradij = request.Oradij
        };

        _db.Edzok.Add(edzo);
        await _db.SaveChangesAsync();
        await _db.Entry(edzo).Reference(e => e.Dolgozo).LoadAsync();
        return ToResponse(edzo);
    }

    public async Task<EdzoResponse> UpdateEdzoAsync(int edzoId, EdzoUpsertRequest request)
    {
        var edzo = await _db.Edzok.Include(e => e.Dolgozo).FirstOrDefaultAsync(e => e.Id == edzoId)
            ?? throw ApiException.NotFound("Edző nem található.");

        edzo.Szakterulet = request.Szakterulet;
        edzo.Vegzettseg = request.Vegzettseg;
        edzo.Oradij = request.Oradij;

        await _db.SaveChangesAsync();
        return ToResponse(edzo);
    }

    public async Task<List<EdzoElerhetosegResponse>> GetElerhetosegAsync(int edzoId)
    {
        var elerhetosegek = await _db.EdzoElerhetosegek.Where(e => e.EdzoId == edzoId).ToListAsync();
        return elerhetosegek
            .Select(e => new EdzoElerhetosegResponse(e.Id, e.HetNapja.ToString(), e.TolIdo, e.IgIdo))
            .ToList();
    }

    public async Task<EdzoElerhetosegResponse> AddElerhetosegAsync(int edzoId, EdzoElerhetosegRequest request)
    {
        if (!await _db.Edzok.AnyAsync(e => e.Id == edzoId))
        {
            throw ApiException.NotFound("Edző nem található.");
        }

        var elerhetoseg = new EdzoElerhetoseg
        {
            EdzoId = edzoId,
            HetNapja = Enum.Parse<HetNapja>(request.HetNapja, ignoreCase: true),
            TolIdo = request.TolIdo,
            IgIdo = request.IgIdo
        };

        _db.EdzoElerhetosegek.Add(elerhetoseg);
        await _db.SaveChangesAsync();
        return new EdzoElerhetosegResponse(elerhetoseg.Id, elerhetoseg.HetNapja.ToString(), elerhetoseg.TolIdo, elerhetoseg.IgIdo);
    }
}
