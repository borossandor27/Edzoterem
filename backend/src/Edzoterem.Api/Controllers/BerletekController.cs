using Edzoterem.Api.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edzoterem.Api.Controllers;

[ApiController]
[Authorize]
public class BerletekController : ControllerBase
{
    private readonly IBerletekService _berletekService;

    public BerletekController(IBerletekService berletekService)
    {
        _berletekService = berletekService;
    }

    [HttpGet("api/berlettipusok")]
    public async Task<IActionResult> GetTipusok() => Ok(await _berletekService.GetTipusokAsync());

    [HttpPost("api/berlettipusok")]
    [Authorize(Roles = "Tulajdonos")]
    public async Task<IActionResult> CreateTipus(BerletTipusUpsertRequest request) =>
        Ok(await _berletekService.CreateTipusAsync(request));

    [HttpPut("api/berlettipusok/{id:int}")]
    [Authorize(Roles = "Tulajdonos")]
    public async Task<IActionResult> UpdateTipus(int id, BerletTipusUpsertRequest request) =>
        Ok(await _berletekService.UpdateTipusAsync(id, request));

    [HttpPost("api/berletek")]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> Hozzarendeles(BerletHozzarendelesRequest request)
    {
        var response = await _berletekService.HozzarendelesAsync(request, User.GetFelhasznaloId());
        return Ok(response);
    }

    [HttpGet("api/berletek/lejaro")]
    public async Task<IActionResult> GetLejarok() => Ok(await _berletekService.GetLejarokAsync());
}
