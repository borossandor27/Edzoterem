using Edzoterem.Api.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edzoterem.Api.Controllers;

[ApiController]
[Route("api/csoportos-foglalkozasok")]
[Authorize]
public class CsoportosFoglalkozasokController : ControllerBase
{
    private readonly ICsoportosFoglalkozasokService _csoportosService;
    private readonly IDolgozokService _dolgozokService;

    public CsoportosFoglalkozasokController(ICsoportosFoglalkozasokService csoportosService, IDolgozokService dolgozokService)
    {
        _csoportosService = csoportosService;
        _dolgozokService = dolgozokService;
    }

    [HttpGet]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> GetAll() => Ok(await _csoportosService.GetAllAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Tulajdonos,Recepcios,Edzo")]
    public async Task<IActionResult> GetById(int id) => Ok(await _csoportosService.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Edzo")]
    public async Task<IActionResult> Meghirdetes(CsoportosFoglalkozasMeghirdetesRequest request)
    {
        var dolgozoId = User.GetDolgozoIdRequired();
        var edzoId = await _dolgozokService.GetEdzoIdByDolgozoIdAsync(dolgozoId);
        var response = await _csoportosService.MeghirdetesAsync(edzoId, User.GetFelhasznaloId(), request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}/jovahagyas")]
    [Authorize(Roles = "Tulajdonos")]
    public async Task<IActionResult> Jovahagyas(int id) =>
        Ok(await _csoportosService.JovahagyasAsync(id, User.GetFelhasznaloId()));

    [HttpPut("{id:int}/elutasitas")]
    [Authorize(Roles = "Tulajdonos")]
    public async Task<IActionResult> Elutasitas(int id, CsoportosFoglalkozasElutasitasRequest request) =>
        Ok(await _csoportosService.ElutasitasAsync(id, User.GetFelhasznaloId(), request));

    [HttpPost("{id:int}/jelentkezes")]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> Jelentkezes(int id, CsoportosJelentkezesRequest request) =>
        Ok(await _csoportosService.JelentkezesAsync(id, request, User.GetFelhasznaloId()));

    [HttpDelete("{id:int}/jelentkezes/{tagId:int}")]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> JelentkezesTorlese(int id, int tagId)
    {
        await _csoportosService.JelentkezesTorleseAsync(id, tagId);
        return NoContent();
    }

    [HttpPut("{id:int}/jelenlet")]
    [Authorize(Roles = "Edzo")]
    public async Task<IActionResult> Jelenlet(int id, JelenletRogzitesRequest request)
    {
        var edzoId = await _dolgozokService.GetEdzoIdByDolgozoIdAsync(User.GetDolgozoIdRequired());
        return Ok(await _csoportosService.JelenletRogzitesAsync(id, edzoId, request));
    }

    [HttpPut("{id:int}/lezaras")]
    [Authorize(Roles = "Edzo")]
    public async Task<IActionResult> Lezaras(int id)
    {
        var edzoId = await _dolgozokService.GetEdzoIdByDolgozoIdAsync(User.GetDolgozoIdRequired());
        return Ok(await _csoportosService.LezarasAsync(id, edzoId));
    }
}
