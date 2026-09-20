using Edzoterem.Application.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Edzoterem.Contracts.Responses;
using Edzoterem.Domain.Entities;
using Edzoterem.Domain.Enums;
using Edzoterem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Edzoterem.Application.Services;

public class TagokService : ITagokService
{
    private readonly EdzoteremDbContext _db;

    public TagokService(EdzoteremDbContext db)
    {
        _db = db;
    }

    private static TagResponse ToResponse(Tag t) => new(
        t.Id, t.Vezeteknev, t.Keresztnev, t.SzuletesiDatum, t.Cim, t.Telefon, t.Email,
        t.RegisztracioDatuma, t.VhKapcsolattartoNev, t.VhKapcsolattartoTelefon, t.Nem.ToString(), t.Megjegyzes, t.Aktiv);

    public async Task<List<TagResponse>> GetAllAsync()
    {
        var tagok = await _db.Tagok.ToListAsync();
        return tagok.Select(ToResponse).ToList();
    }

    public async Task<TagResponse> GetByIdAsync(int id)
    {
        var tag = await _db.Tagok.FindAsync(id) ?? throw ApiException.NotFound("Tag nem található.");
        return ToResponse(tag);
    }

    public async Task<TagResponse> GetSajatAdataimAsync(int tagId) => await GetByIdAsync(tagId);

    public async Task<TagRegisztracioValaszResponse> RegisztracioAsync(TagRegisztracioRequest request)
    {
        var tag = new Tag
        {
            Vezeteknev = request.Vezeteknev,
            Keresztnev = request.Keresztnev,
            SzuletesiDatum = request.SzuletesiDatum,
            Cim = request.Cim,
            Telefon = request.Telefon,
            Email = request.Email,
            RegisztracioDatuma = DateOnly.FromDateTime(DateTime.UtcNow),
            VhKapcsolattartoNev = request.VhKapcsolattartoNev,
            VhKapcsolattartoTelefon = request.VhKapcsolattartoTelefon,
            Nem = Enum.Parse<Nem>(request.Nem, ignoreCase: true),
            Megjegyzes = request.Megjegyzes
        };

        _db.Tagok.Add(tag);
        await _db.SaveChangesAsync();

        var felhasznalonev = await GeneraljEgyediFelhasznalonevAsync(tag.Vezeteknev, tag.Keresztnev);
        var ideiglenesJelszo = JelszoGeneralo.Generalas();

        var felhasznalo = new Felhasznalo
        {
            Felhasznalonev = felhasznalonev,
            JelszoHash = BCrypt.Net.BCrypt.HashPassword(ideiglenesJelszo),
            Szerepkor = Szerepkor.Tag,
            TagId = tag.Id,
            JelszoIdeiglenes = true
        };

        _db.Felhasznalok.Add(felhasznalo);
        await _db.SaveChangesAsync();

        return new TagRegisztracioValaszResponse(ToResponse(tag), felhasznalonev, ideiglenesJelszo);
    }

    private async Task<string> GeneraljEgyediFelhasznalonevAsync(string vezeteknev, string keresztnev)
    {
        var alap = $"{vezeteknev}.{keresztnev}".ToLowerInvariant().Replace(" ", "");
        var jelolt = alap;
        var i = 1;
        while (await _db.Felhasznalok.AnyAsync(f => f.Felhasznalonev == jelolt))
        {
            jelolt = $"{alap}{++i}";
        }

        return jelolt;
    }

    public async Task<TagResponse> UpdateAsync(int id, TagUpdateRequest request)
    {
        var tag = await _db.Tagok.FindAsync(id) ?? throw ApiException.NotFound("Tag nem található.");

        tag.Vezeteknev = request.Vezeteknev;
        tag.Keresztnev = request.Keresztnev;
        tag.SzuletesiDatum = request.SzuletesiDatum;
        tag.Cim = request.Cim;
        tag.Telefon = request.Telefon;
        tag.Email = request.Email;
        tag.VhKapcsolattartoNev = request.VhKapcsolattartoNev;
        tag.VhKapcsolattartoTelefon = request.VhKapcsolattartoTelefon;
        tag.Nem = Enum.Parse<Nem>(request.Nem, ignoreCase: true);
        tag.Megjegyzes = request.Megjegyzes;

        await _db.SaveChangesAsync();
        return ToResponse(tag);
    }

    public async Task InaktivalasAsync(int id)
    {
        var tag = await _db.Tagok.FindAsync(id) ?? throw ApiException.NotFound("Tag nem található.");
        tag.Aktiv = false;
        await _db.SaveChangesAsync();
    }
}
