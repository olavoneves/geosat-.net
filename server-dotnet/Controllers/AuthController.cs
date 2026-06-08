using GeoSat.API.DTOs.Request;
using GeoSat.API.Models;
using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request);
        return Ok(token);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var token = await _authService.RefreshAsync(request);
        return Ok(token);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        var usuario = (UsuarioNet)HttpContext.Items["Usuario"]!;
        await _authService.LogoutAsync(usuario);
        return NoContent();
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register([FromBody] UsuarioRequest request)
    {
        var usuario = (UsuarioNet)HttpContext.Items["Usuario"]!;
        if (usuario.DsRole != "ADMIN")
            return Forbid();

        var novo = await _authService.RegisterAsync(request);
        return CreatedAtAction(nameof(Register), new { id = novo.IdUsuario }, novo);
    }
}
