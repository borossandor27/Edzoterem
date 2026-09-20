using Edzoterem.Api.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edzoterem.Api.Controllers;

[ApiController]
[Route("api/tagok")]
[Authorize]
public class TagokController : ControllerBase
{
    private readonly ITagokService _tagokService;
    private readonly IBerletekService _berletekService;
    private readonly ICsoportosFoglalkozasokService _csoportosService;
    private readonly IEgyeniFoglalkozasokService _egyeniService;

    public TagokController(
        ITagokService tagokService,
        IBerletekService berletekService,
        ICsoportosFoglalkozasokService csoportosService,
        IEgyeniFoglalkozasokService egyeniService)
    {
        _tagokService = tagokService;
        _berletekService = berletekService;
        _csoportosService = csoportosService;
        _egyeniService = egyeniService;
    }

    [HttpGet]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> GetAll() => Ok(await _tagokService.GetAllAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Tulajdonos,Recepcios,Edzo")]
    public async Task<IActionResult> GetById(int id) => Ok(await _tagokService.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Tulajdonos,Recepcios")]
    public async Task<IActionResult> Regisztracio(TagRegisztracioRequest request)
    {
        var response = await _tagokService.RegisztracioAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Tag.Id }, response);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Tulajdonos,Recepcios,Edzo")]
    public async Task<IActionResult> Update(int id, TagUpdateRequest request) =>
        Ok(await _tagokService.UpdateAsync(id, request));

    [HttpPut("{id:int}/inaktivalas")]
    [Authorize(Roles = "Tulajdonos")]
    public async Task<IActionResult> Inaktivalas(int id)
    {
        await _tagokService.InaktivalasAsync(id);
        return NoContent();
    }

    [HttpGet("{id:int}/berletek")]
    [Authorize(Roles = "Tulajdonos,Recepcios,Edzo")]
    public async Task<IActionResult> GetBerletek(int id) => Ok(await _berletekService.GetTagBerleteiAsync(id));

    [HttpGet("me")]
    [Authorize(Roles = "Tag")]
    public async Task<IActionResult> GetSajatAdataim() => Ok(await _tagokService.GetSajatAdataimAsync(User.GetTagIdRequired()));

    [HttpGet("me/berlet")]
    [Authorize(Roles = "Tag")]
    public async Task<IActionResult> GetSajatBerletek() => Ok(await _berletekService.GetTagBerleteiAsync(User.GetTagIdRequired()));

    [HttpGet("me/csoportos-foglalkozasok")]
    [Authorize(Roles = "Tag")]
    public async Task<IActionResult> GetSajatCsoportosFoglalkozasok() =>
        Ok(await _csoportosService.GetTagSajatjaiAsync(User.GetTagIdRequired()));

    [HttpGet("me/egyeni-foglalkozasok")]
    [Authorize(Roles = "Tag")]
    public async Task<IActionResult> GetSajatEgyeniFoglalkozasok() =>
        Ok(await _egyeniService.GetTagSajatjaiAsync(User.GetTagIdRequired()));
}
