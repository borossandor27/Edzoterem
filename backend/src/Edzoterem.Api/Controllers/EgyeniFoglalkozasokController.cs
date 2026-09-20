using Edzoterem.Api.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edzoterem.Api.Controllers;

[ApiController]
[Authorize]
public class EgyeniFoglalkozasokController : ControllerBase
{
    private readonly IEgyeniFoglalkozasokService _egyeniService;

    public EgyeniFoglalkozasokController(IEgyeniFoglalkozasokService egyeniService)
    {
        _egyeniService = egyeniService;
    }

    [HttpGet("api/szolgaltatastipusok")]
    public async Task<IActionResult> GetSzolgaltatasTipusok() => Ok(await _egyeniService.GetSzolgaltatasTipusokAsync());

    [HttpPost("api/szolgaltatastipusok")]
    [Authorize(Roles = "Tulajdonos")]
    public async Task<IActionResult> CreateSzolgaltatasTipus(SzolgaltatasTipusUpsertRequest request) =>
        Ok(await _egyeniService.CreateSzolgaltatasTipusAsync(request));

    [HttpPost("api/egyeni-foglalkozasok")]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> Rogzites(EgyeniFoglalkozasRequest request) =>
        Ok(await _egyeniService.RogzitesAsync(request, User.GetFelhasznaloId()));

    [HttpPut("api/egyeni-foglalkozasok/{id:int}/statusz")]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> StatuszFrissites(int id, EgyeniFoglalkozasStatuszRequest request) =>
        Ok(await _egyeniService.StatuszFrissitesAsync(id, request));
}
