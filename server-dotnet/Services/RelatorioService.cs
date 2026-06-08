using GeoSat.API.Data;
using GeoSat.API.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Services;

public class RelatorioService
{
    private readonly AppDbContext _db;

    public RelatorioService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<RelatorioAlertaResponse>> AlertasPorProdutorAsync(
        int idProdutor, DateTime? inicio, DateTime? fim)
    {
        var produtor = await _db.Produtores.FindAsync(idProdutor)
            ?? throw new KeyNotFoundException($"Produtor {idProdutor} não encontrado");

        var dataInicio = inicio ?? DateTime.UtcNow.AddDays(-30);
        var dataFim = fim ?? DateTime.UtcNow;

        var talhoes = await _db.Talhoes
            .Include(t => t.Propriedade)
            .Include(t => t.Alertas.Where(a => a.DtGerado >= dataInicio && a.DtGerado <= dataFim))
            .Where(t => t.Propriedade.IdProdutor == idProdutor && t.FlAtivo == "S")
            .ToListAsync();

        return talhoes.Select(t =>
        {
            var alertas = t.Alertas.ToList();
            return new RelatorioAlertaResponse(
                t.IdTalhao,
                t.NmNome,
                t.DsCultura,
                alertas.Count,
                alertas.Count(a => a.StStatus == "PENDENTE"),
                alertas.Count(a => a.StStatus == "VISUALIZADO"),
                alertas.Count(a => a.StStatus == "RESOLVIDO"),
                alertas.Count(a => a.TpNivel == "ATENCAO"),
                alertas.Count(a => a.TpNivel == "ALERTA"),
                alertas.Count(a => a.TpNivel == "CRITICO")
            );
        });
    }

    public async Task<TempoRespostaResponse> TempoRespostaAsync(int idTalhao)
    {
        var talhao = await _db.Talhoes.FindAsync(idTalhao)
            ?? throw new KeyNotFoundException($"Talhão {idTalhao} não encontrado");

        var alertas = await _db.Alertas
            .Where(a => a.IdTalhao == idTalhao)
            .ToListAsync();

        var comVisualizacao = alertas.Where(a => a.DtVisualizado.HasValue).ToList();
        var comResolucao = alertas.Where(a => a.DtResolvido.HasValue).ToList();

        double? tempoMedioViz = comVisualizacao.Count > 0
            ? comVisualizacao.Average(a => (a.DtVisualizado!.Value - a.DtGerado).TotalHours)
            : null;

        double? tempoMedioRes = comResolucao.Count > 0
            ? comResolucao.Average(a => (a.DtResolvido!.Value - a.DtGerado).TotalHours)
            : null;

        return new TempoRespostaResponse(
            idTalhao,
            talhao.NmNome,
            tempoMedioViz.HasValue ? Math.Round(tempoMedioViz.Value, 2) : null,
            tempoMedioRes.HasValue ? Math.Round(tempoMedioRes.Value, 2) : null,
            comResolucao.Count
        );
    }

    public async Task<NdviHistoricoResponse> NdviHistoricoAsync(int idTalhao)
    {
        var talhao = await _db.Talhoes.FindAsync(idTalhao)
            ?? throw new KeyNotFoundException($"Talhão {idTalhao} não encontrado");

        var limite = DateTime.UtcNow.AddDays(-90);
        var historico = await _db.ImagensSatelitais
            .Where(i => i.IdTalhao == idTalhao && i.DsStatusProc == "PROCESSADO"
                        && i.NrNdvi.HasValue && i.DtCaptura >= limite)
            .OrderBy(i => i.DtCaptura)
            .Select(i => new NdviPontoResponse(i.DtCaptura, i.NrNdvi!.Value, i.DsFonte))
            .ToListAsync();

        return new NdviHistoricoResponse(idTalhao, talhao.NmNome, historico);
    }
}
