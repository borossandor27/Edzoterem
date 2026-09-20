using Edzoterem.Api.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edzoterem.Api.Controllers;

[ApiController]
[Route("api/edzok")]
[Authorize]
public class EdzokController : ControllerBase
{
    private readonly IDolgozokService _dolgozokService;
    private readonly ICsoportosFoglalkozasokService _csoportosService;
    private readonly IEgyeniFoglalkozasokService _egyeniService;

    public EdzokController(
        IDolgozokService dolgozokService,
        ICsoportosFoglalkozasokService csoportosService,
        IEgyeniFoglalkozasokService egyeniService)
    {
        _dolgozokService = dolgozokService;
        _csoportosService = csoportosService;
        _egyeniService = egyeniService;
    }

    [HttpGet]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> GetAll() => Ok(await _dolgozokService.GetEdzokAsync());

    [HttpPost("{dolgozoId:int}")]
    [Authorize(Roles = "Tulajdonos")]
    public async Task<IActionResult> Create(int dolgozoId, EdzoUpsertRequest request) =>
        Ok(await _dolgozokService.CreateEdzoAsync(dolgozoId, request));

    [HttpPut("{edzoId:int}")]
    [Authorize(Roles = "Tulajdonos")]
    public async Task<IActionResult> Update(int edzoId, EdzoUpsertRequest request) =>
        Ok(await _dolgozokService.UpdateEdzoAsync(edzoId, request));

    [HttpGet("{edzoId:int}/elerhetoseg")]
    [Authorize(Roles = "Tulajdonos,Recepcios,Edzo")]
    public async Task<IActionResult> GetElerhetoseg(int edzoId) => Ok(await _dolgozokService.GetElerhetosegAsync(edzoId));

    [HttpPut("{edzoId:int}/elerhetoseg")]
    [Authorize(Roles = "Tulajdonos,Edzo")]
    public async Task<IActionResult> AddElerhetoseg(int edzoId, EdzoElerhetosegRequest request)
    {
        if (User.IsInRole("Edzo"))
        {
            var sajatEdzoId = await _dolgozokService.GetEdzoIdByDolgozoIdAsync(User.GetDolgozoIdRequired());
            if (sajatEdzoId != edzoId)
            {
                return Forbid();
            }
        }

        return Ok(await _dolgozokService.AddElerhetosegAsync(edzoId, request));
    }

    [HttpGet("me/csoportos-foglalkozasok")]
    [Authorize(Roles = "Edzo")]
    public async Task<IActionResult> GetSajatCsoportosFoglalkozasok()
    {
        var edzoId = await _dolgozokService.GetEdzoIdByDolgozoIdAsync(User.GetDolgozoIdRequired());
        return Ok(await _csoportosService.GetEdzoSajatjaiAsync(edzoId));
    }

    [HttpGet("me/egyeni-foglalkozasok")]
    [Authorize(Roles = "Edzo")]
    public async Task<IActionResult> GetSajatEgyeniFoglalkozasok()
    {
        var dolgozoId = User.GetDolgozoIdRequired();
        return Ok(await _egyeniService.GetDolgozoSajatjaiAsync(dolgozoId));
    }
}
