using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("relatorios")]
public class RelatorioController : ControllerBase
{
    private readonly RelatorioService _relatorioService;

    public RelatorioController(RelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet("alertas-por-produtor/{idProdutor:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlertasPorProdutor(
        int idProdutor,
        [FromQuery] DateTime? inicio,
        [FromQuery] DateTime? fim)
    {
        var result = await _relatorioService.AlertasPorProdutorAsync(idProdutor, inicio, fim);
        return Ok(result);
    }

    [HttpGet("tempo-resposta/{idTalhao:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TempoResposta(int idTalhao)
    {
        var result = await _relatorioService.TempoRespostaAsync(idTalhao);
        return Ok(result);
    }

    [HttpGet("ndvi-historico/{idTalhao:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> NdviHistorico(int idTalhao)
    {
        var result = await _relatorioService.NdviHistoricoAsync(idTalhao);
        return Ok(result);
    }
}
