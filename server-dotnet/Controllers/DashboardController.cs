using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("dashboard")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("resumo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Resumo()
    {
        var result = await _dashboardService.ResumoAsync();
        return Ok(result);
    }

    [HttpGet("alertas-por-nivel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AlertasPorNivel()
    {
        var result = await _dashboardService.AlertasPorNivelAsync();
        return Ok(result);
    }

    [HttpGet("talhoes-em-risco")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> TalhoesEmRisco()
    {
        var result = await _dashboardService.TalhoesEmRiscoAsync();
        return Ok(result);
    }

    [HttpGet("ndvi-medio/{idTalhao:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> NdviMedio(int idTalhao)
    {
        var result = await _dashboardService.NdviMedioPorTalhaoAsync(idTalhao);
        return Ok(result);
    }
}
