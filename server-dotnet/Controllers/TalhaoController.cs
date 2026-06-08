using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("talhoes")]
public class TalhaoController : ControllerBase
{
    private readonly TalhaoService _talhaoService;

    public TalhaoController(TalhaoService talhaoService)
    {
        _talhaoService = talhaoService;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var result = await _talhaoService.BuscarPorIdAsync(id);
        return Ok(result);
    }

    [HttpGet("propriedade/{idPropriedade:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarPorPropriedade(int idPropriedade)
    {
        var result = await _talhaoService.ListarPorPropriedadeAsync(idPropriedade);
        return Ok(result);
    }
}
