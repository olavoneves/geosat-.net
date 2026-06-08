using GeoSat.API.Data;
using GeoSat.API.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Services;

public class DashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardResponse> ResumoAsync()
    {
        var totalProdutores = await _db.Produtores.CountAsync(p => p.FlAtivo == "S");
        var totalPropriedades = await _db.Propriedades.CountAsync(p => p.FlAtiva == "S");
        var totalTalhoes = await _db.Talhoes.CountAsync(t => t.FlAtivo == "S");
        var alertasPendentes = await _db.Alertas.CountAsync(a => a.StStatus == "PENDENTE");
        var alertasCriticos = await _db.Alertas.CountAsync(a => a.TpNivel == "CRITICO" && a.StStatus == "PENDENTE");
        var imagensPendentes = await _db.ImagensSatelitais.CountAsync(i => i.DsStatusProc == "PENDENTE");

        return new DashboardResponse(
            totalProdutores, totalPropriedades, totalTalhoes,
            alertasPendentes, alertasCriticos, imagensPendentes
        );
    }

    public async Task<AlertasPorNivelResponse> AlertasPorNivelAsync()
    {
        var atencao = await _db.Alertas.CountAsync(a => a.TpNivel == "ATENCAO" && a.StStatus == "PENDENTE");
        var alerta = await _db.Alertas.CountAsync(a => a.TpNivel == "ALERTA" && a.StStatus == "PENDENTE");
        var critico = await _db.Alertas.CountAsync(a => a.TpNivel == "CRITICO" && a.StStatus == "PENDENTE");

        return new AlertasPorNivelResponse(atencao, alerta, critico);
    }

    public async Task<IEnumerable<TalhaoEmRiscoResponse>> TalhoesEmRiscoAsync()
    {
        var talhoes = await _db.Talhoes
            .Include(t => t.Alertas)
            .Include(t => t.Propriedade)
            .Where(t => t.FlAtivo == "S" && t.Alertas.Any(a =>
                (a.TpNivel == "CRITICO" || a.TpNivel == "ALERTA") && a.StStatus == "PENDENTE"))
            .ToListAsync();

        return talhoes.Select(t =>
        {
            var alertasPendentes = t.Alertas.Where(a => a.StStatus == "PENDENTE").ToList();
            var piorNivel = alertasPendentes.Any(a => a.TpNivel == "CRITICO") ? "CRITICO" : "ALERTA";

            return new TalhaoEmRiscoResponse(
                t.IdTalhao,
                t.NmNome,
                t.DsCultura,
                t.Propriedade.NmNome,
                t.Propriedade.NmMunicipio,
                t.Propriedade.SgEstado,
                alertasPendentes.Count,
                piorNivel
            );
        });
    }

    public async Task<NdviMedioResponse> NdviMedioPorTalhaoAsync(int idTalhao)
    {
        var talhao = await _db.Talhoes.FindAsync(idTalhao)
            ?? throw new KeyNotFoundException($"Talhão {idTalhao} não encontrado");

        var limite = DateTime.UtcNow.AddDays(-30);
        var imagens = await _db.ImagensSatelitais
            .Where(i => i.IdTalhao == idTalhao && i.DsStatusProc == "PROCESSADO" && i.DtCaptura >= limite)
            .ToListAsync();

        decimal? ndviMedio = imagens.Count > 0
            ? imagens.Where(i => i.NrNdvi.HasValue).Average(i => i.NrNdvi)
            : null;

        return new NdviMedioResponse(
            idTalhao,
            talhao.NmNome,
            ndviMedio,
            imagens.Count,
            imagens.Count > 0 ? imagens.Max(i => i.DtCaptura) : null
        );
    }
}
