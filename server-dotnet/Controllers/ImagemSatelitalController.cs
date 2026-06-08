using GeoSat.API.DTOs.Request;
using GeoSat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoSat.API.Controllers;

[ApiController]
[Route("imagens")]
public class ImagemSatelitalController : ControllerBase
{
    private readonly ImagemSatelitalService _imagemService;

    public ImagemSatelitalController(ImagemSatelitalService imagemService)
    {
        _imagemService = imagemService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Registrar([FromBody] ImagemSatelitalRequest request)
    {
        var result = await _imagemService.RegistrarAsync(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = result.IdImagem }, result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var result = await _imagemService.BuscarPorIdAsync(id);
        return Ok(result);
    }

    [HttpGet("talhao/{idTalhao:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPorTalhao(
        int idTalhao,
        [FromQuery] int page = 1,
        [FromQuery] int size = 20)
    {
        var (items, total) = await _imagemService.ListarPorTalhaoAsync(idTalhao, page, size);
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("pendentes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPendentes()
    {
        var result = await _imagemService.ListarPendentesAsync();
        return Ok(result);
    }

    [HttpPatch("{id:int}/processar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Processar(int id, [FromBody] ProcessarImagemRequest request)
    {
        var result = await _imagemService.ProcessarAsync(id, request);
        return Ok(result);
    }

    [HttpPatch("{id:int}/erro")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarErro(int id, [FromBody] ErroImagemRequest request)
    {
        var result = await _imagemService.RegistrarErroAsync(id, request);
        return Ok(result);
    }
}
