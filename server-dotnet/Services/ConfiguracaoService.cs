using GeoSat.API.Data;
using GeoSat.API.DTOs.Request;
using GeoSat.API.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Services;

public class ConfiguracaoService
{
    private readonly AppDbContext _db;

    public ConfiguracaoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ConfiguracaoResponse> BuscarPorTalhaoAsync(int idTalhao)
    {
        var config = await _db.Configuracoes
            .Include(c => c.Talhao)
            .FirstOrDefaultAsync(c => c.IdTalhao == idTalhao)
            ?? throw new KeyNotFoundException($"Configuração do talhão {idTalhao} não encontrada");

        return MapToResponse(config);
    }

    public async Task<ConfiguracaoResponse> AtualizarAsync(int id, ConfiguracaoRequest request)
    {
        var config = await _db.Configuracoes
            .Include(c => c.Talhao)
            .FirstOrDefaultAsync(c => c.IdConfig == id)
            ?? throw new KeyNotFoundException($"Configuração {id} não encontrada");

        config.NrThresholdUmidMin = request.NrThresholdUmidMin;
        config.NrThresholdNdviMin = request.NrThresholdNdviMin;
        config.NrJanelaFusaoHoras = request.NrJanelaFusaoHoras;
        config.DtAtualizacao = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return MapToResponse(config);
    }

    private static ConfiguracaoResponse MapToResponse(Models.Configuracao c) =>
        new(c.IdConfig, c.IdTalhao, c.Talhao.NmNome,
            c.NrThresholdUmidMin, c.NrThresholdNdviMin, c.NrJanelaFusaoHoras, c.DtAtualizacao);
}
