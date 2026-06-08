using GeoSat.API.Models;
using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("alertas")]
public class AlertaController : ControllerBase
{
    private readonly AlertaService _alertaService;

    public AlertaController(AlertaService alertaService)
    {
        _alertaService = alertaService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] string? status,
        [FromQuery] string? nivel,
        [FromQuery] int page = 1,
        [FromQuery] int size = 20)
    {
        var (items, total) = await _alertaService.ListarAsync(status, nivel, page, size);
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var result = await _alertaService.BuscarPorIdAsync(id);
        return Ok(result);
    }

    [HttpGet("talhao/{idTalhao:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPorTalhao(int idTalhao)
    {
        var result = await _alertaService.ListarPorTalhaoAsync(idTalhao);
        return Ok(result);
    }

    [HttpPatch("{id:int}/visualizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Visualizar(int id)
    {
        var result = await _alertaService.VisualizarAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id:int}/resolver")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Resolver(int id)
    {
        var result = await _alertaService.ResolverAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id:int}/reabrir")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reabrir(int id)
    {
        var usuario = (UsuarioNet)HttpContext.Items["Usuario"]!;
        if (usuario.DsRole != "ADMIN")
            return Forbid();

        var result = await _alertaService.ReabrirAsync(id);
        return Ok(result);
    }
}
