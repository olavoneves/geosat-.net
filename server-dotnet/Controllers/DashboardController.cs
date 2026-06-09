using GeoSat.API.DTOs.Response;
using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("dashboard")]
[Tags("Dashboard — Painel do Gestor")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("resumo")]
    [EndpointSummary("Resumo geral do sistema")]
    [EndpointDescription(
        "Visão macro para o gestor da cooperativa. Retorna em tempo real:\n" +
        "- totalProdutores: produtores ativos cadastrados\n" +
        "- totalPropriedades: propriedades ativas no sistema\n" +
        "- totalTalhoes: talhões monitorados\n" +
        "- alertasPendentes: alertas aguardando ação do produtor\n" +
        "- alertasCriticos: alertas de nível CRITICO pendentes (fusão sensor + satélite)\n" +
        "- imagensPendentes: imagens satelitais aguardando processamento de NDVI")]
    [ProducesResponseType(typeof(DashboardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Resumo()
    {
        var result = await _dashboardService.ResumoAsync();
        return Ok(result);
    }

    [HttpGet("alertas-por-nivel")]
    [EndpointSummary("Distribuição de alertas por nível de severidade")]
    [EndpointDescription(
        "Retorna a contagem de alertas PENDENTES agrupados por nível:\n" +
        "- ATENCAO: threshold cruzado levemente — monitorar\n" +
        "- ALERTA: threshold cruzado significativamente — intervir em breve\n" +
        "- CRITICO: fusão confirmada (sensor + satélite concordam) — ação imediata\n\n" +
        "Inclui apenas alertas com status PENDENTE. Use para priorizar ações no campo.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AlertasPorNivel()
    {
        var result = await _dashboardService.AlertasPorNivelAsync();
        return Ok(result);
    }

    [HttpGet("talhoes-em-risco")]
    [EndpointSummary("Talhões com alertas críticos ou de alerta pendentes")]
    [EndpointDescription(
        "Lista talhões que possuem pelo menos um alerta de nível CRITICO ou ALERTA " +
        "com status PENDENTE. Ordenado por data do alerta mais recente.\n\n" +
        "Um alerta CRITICO indica fusão: tanto o sensor ESP32 quanto a imagem satelital " +
        "confirmaram o problema dentro da janela de tempo configurada para o talhão. " +
        "Esses talhões exigem atenção prioritária do gestor.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> TalhoesEmRisco()
    {
        var result = await _dashboardService.TalhoesEmRiscoAsync();
        return Ok(result);
    }

    [HttpGet("ndvi-medio/{idTalhao:int}")]
    [EndpointSummary("NDVI médio do talhão nos últimos 30 dias")]
    [EndpointDescription(
        "Calcula a média do índice NDVI (Normalized Difference Vegetation Index) das " +
        "imagens processadas do talhão nos últimos 30 dias.\n\n" +
        "Escala NDVI:\n" +
        "- 0.6 a 1.0: vegetação densa e saudável\n" +
        "- 0.3 a 0.6: vegetação moderada\n" +
        "- 0.0 a 0.3: vegetação escassa ou estressada — zona de alerta\n" +
        "- Abaixo de 0: solo exposto, água ou nuvem\n\n" +
        "Retorna null se não houver imagens com status PROCESSADO no período.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> NdviMedio(int idTalhao)
    {
        var result = await _dashboardService.NdviMedioPorTalhaoAsync(idTalhao);
        return Ok(result);
    }
}
