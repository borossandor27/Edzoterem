using Edzoterem.Api.Common;
using Edzoterem.Application.Interfaces;
using Edzoterem.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edzoterem.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var response = await _authService.GetSajatFelhasznaloAsync(User.GetFelhasznaloId());
        return Ok(response);
    }

    [HttpPost("jelszo-valtoztatas")]
    [Authorize]
    public async Task<IActionResult> JelszoValtoztatas(JelszoValtoztatasRequest request)
    {
        await _authService.JelszoValtoztatasAsync(User.GetFelhasznaloId(), request);
        return NoContent();
    }
}
