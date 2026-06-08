using GeoSat.API.DTOs.Request;
using GeoSat.API.Models;
using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    private IActionResult CheckAdmin()
    {
        var usuario = (UsuarioNet)HttpContext.Items["Usuario"]!;
        return usuario.DsRole != "ADMIN" ? Forbid() : null!;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var check = CheckAdmin();
        if (check != null) return check;

        var result = await _usuarioService.ListarAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var check = CheckAdmin();
        if (check != null) return check;

        var result = await _usuarioService.BuscarPorIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioUpdateRequest request)
    {
        var check = CheckAdmin();
        if (check != null) return check;

        var result = await _usuarioService.AtualizarAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(int id)
    {
        var check = CheckAdmin();
        if (check != null) return check;

        await _usuarioService.DesativarAsync(id);
        return NoContent();
    }
}
