using GeoSat.API.DTOs.Request;
using GeoSat.API.Models;
using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("configuracoes")]
public class ConfiguracaoController : ControllerBase
{
    private readonly ConfiguracaoService _configuracaoService;

    public ConfiguracaoController(ConfiguracaoService configuracaoService)
    {
        _configuracaoService = configuracaoService;
    }

    [HttpGet("talhao/{idTalhao:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorTalhao(int idTalhao)
    {
        var result = await _configuracaoService.BuscarPorTalhaoAsync(idTalhao);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] ConfiguracaoRequest request)
    {
        var usuario = (UsuarioNet)HttpContext.Items["Usuario"]!;
        if (usuario.DsRole != "ADMIN")
            return Forbid();

        var result = await _configuracaoService.AtualizarAsync(id, request);
        return Ok(result);
    }
}
