using GeoSat.API.Data;
using GeoSat.API.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Services;

public class AlertaService
{
    private readonly AppDbContext _db;

    public AlertaService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(IEnumerable<AlertaResponse> Items, int Total)> ListarAsync(
        string? status, string? nivel, int page, int size)
    {
        var query = _db.Alertas
            .Include(a => a.Talhao)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(a => a.StStatus == status.ToUpper());

        if (!string.IsNullOrEmpty(nivel))
            query = query.Where(a => a.TpNivel == nivel.ToUpper());

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.DtGerado)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(a => new AlertaResponse(
                a.IdAlerta, a.IdTalhao, a.Talhao.NmNome,
                a.TpTipo, a.TpNivel, a.TpOrigem, a.DsDescricao,
                a.StStatus, a.DtGerado, a.DtVisualizado, a.DtResolvido))
            .ToListAsync();

        return (items, total);
    }

    public async Task<AlertaDetalheResponse> BuscarPorIdAsync(int id)
    {
        var alerta = await _db.Alertas
            .Include(a => a.Talhao)
            .Include(a => a.Logs.OrderByDescending(l => l.DtEvento))
            .FirstOrDefaultAsync(a => a.IdAlerta == id)
            ?? throw new KeyNotFoundException($"Alerta {id} não encontrado");

        var logs = alerta.Logs.Select(l => new LogAlertaResponse(
            l.IdLog, l.DsAcao, l.DsOrigem, l.DsObservacao, l.DtEvento));

        return new AlertaDetalheResponse(
            alerta.IdAlerta, alerta.IdTalhao, alerta.Talhao.NmNome,
            alerta.TpTipo, alerta.TpNivel, alerta.TpOrigem, alerta.DsDescricao,
            alerta.StStatus, alerta.DtGerado, alerta.DtVisualizado, alerta.DtResolvido,
            logs);
    }

    public async Task<IEnumerable<AlertaResponse>> ListarPorTalhaoAsync(int idTalhao)
    {
        return await _db.Alertas
            .Include(a => a.Talhao)
            .Where(a => a.IdTalhao == idTalhao)
            .OrderByDescending(a => a.DtGerado)
            .Select(a => new AlertaResponse(
                a.IdAlerta, a.IdTalhao, a.Talhao.NmNome,
                a.TpTipo, a.TpNivel, a.TpOrigem, a.DsDescricao,
                a.StStatus, a.DtGerado, a.DtVisualizado, a.DtResolvido))
            .ToListAsync();
    }

    public async Task<AlertaResponse> VisualizarAsync(int id)
    {
        var alerta = await _db.Alertas
            .Include(a => a.Talhao)
            .FirstOrDefaultAsync(a => a.IdAlerta == id)
            ?? throw new KeyNotFoundException($"Alerta {id} não encontrado");

        if (alerta.StStatus != "PENDENTE")
            throw new InvalidOperationException("Apenas alertas pendentes podem ser visualizados");

        alerta.StStatus = "VISUALIZADO";
        alerta.DtVisualizado = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new AlertaResponse(
            alerta.IdAlerta, alerta.IdTalhao, alerta.Talhao.NmNome,
            alerta.TpTipo, alerta.TpNivel, alerta.TpOrigem, alerta.DsDescricao,
            alerta.StStatus, alerta.DtGerado, alerta.DtVisualizado, alerta.DtResolvido);
    }

    public async Task<AlertaResponse> ResolverAsync(int id)
    {
        var alerta = await _db.Alertas
            .Include(a => a.Talhao)
            .FirstOrDefaultAsync(a => a.IdAlerta == id)
            ?? throw new KeyNotFoundException($"Alerta {id} não encontrado");

        if (alerta.StStatus == "RESOLVIDO")
            throw new InvalidOperationException("Alerta já resolvido");

        alerta.StStatus = "RESOLVIDO";
        alerta.DtResolvido = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new AlertaResponse(
            alerta.IdAlerta, alerta.IdTalhao, alerta.Talhao.NmNome,
            alerta.TpTipo, alerta.TpNivel, alerta.TpOrigem, alerta.DsDescricao,
            alerta.StStatus, alerta.DtGerado, alerta.DtVisualizado, alerta.DtResolvido);
    }

    public async Task<AlertaResponse> ReabrirAsync(int id)
    {
        var alerta = await _db.Alertas
            .Include(a => a.Talhao)
            .FirstOrDefaultAsync(a => a.IdAlerta == id)
            ?? throw new KeyNotFoundException($"Alerta {id} não encontrado");

        if (alerta.StStatus != "RESOLVIDO")
            throw new InvalidOperationException("Apenas alertas resolvidos podem ser reabertos");

        alerta.StStatus = "PENDENTE";
        alerta.DtResolvido = null;
        await _db.SaveChangesAsync();

        return new AlertaResponse(
            alerta.IdAlerta, alerta.IdTalhao, alerta.Talhao.NmNome,
            alerta.TpTipo, alerta.TpNivel, alerta.TpOrigem, alerta.DsDescricao,
            alerta.StStatus, alerta.DtGerado, alerta.DtVisualizado, alerta.DtResolvido);
    }
}
