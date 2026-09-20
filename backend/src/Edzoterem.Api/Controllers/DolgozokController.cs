using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edzoterem.Api.Controllers;

[ApiController]
[Route("api/dolgozok")]
[Authorize(Roles = "Tulajdonos")]
public class DolgozokController : ControllerBase
{
    private readonly IDolgozokService _dolgozokService;

    public DolgozokController(IDolgozokService dolgozokService)
    {
        _dolgozokService = dolgozokService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _dolgozokService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _dolgozokService.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(DolgozoUpsertRequest request)
    {
        var response = await _dolgozokService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DolgozoUpsertRequest request) =>
        Ok(await _dolgozokService.UpdateAsync(id, request));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Kileptetes(int id)
    {
        await _dolgozokService.KileptetesAsync(id);
        return NoContent();
    }

    [HttpGet("{id:int}/munkarend")]
    public async Task<IActionResult> GetMunkarend(int id) => Ok(await _dolgozokService.GetMunkarendAsync(id));

    [HttpPost("{id:int}/munkarend")]
    public async Task<IActionResult> AddMunkarend(int id, MunkarendRequest request) =>
        Ok(await _dolgozokService.AddMunkarendAsync(id, request));
}
