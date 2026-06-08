using GeoSat.API.Data;
using GeoSat.API.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Services;

public class TalhaoService
{
    private readonly AppDbContext _db;

    public TalhaoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TalhaoResponse> BuscarPorIdAsync(int id)
    {
        var talhao = await _db.Talhoes
            .Include(t => t.Propriedade)
            .FirstOrDefaultAsync(t => t.IdTalhao == id)
            ?? throw new KeyNotFoundException($"Talhão {id} não encontrado");

        return MapToResponse(talhao);
    }

    public async Task<IEnumerable<TalhaoResponse>> ListarPorPropriedadeAsync(int idPropriedade)
    {
        var existe = await _db.Propriedades.AnyAsync(p => p.IdPropriedade == idPropriedade);
        if (!existe)
            throw new KeyNotFoundException($"Propriedade {idPropriedade} não encontrada");

        return await _db.Talhoes
            .Include(t => t.Propriedade)
            .Where(t => t.IdPropriedade == idPropriedade && t.FlAtivo == "S")
            .OrderBy(t => t.NmNome)
            .Select(t => new TalhaoResponse(
                t.IdTalhao, t.IdPropriedade, t.NmNome, t.DsCultura,
                t.NrAreaHa, t.FlAtivo, t.DtCriacao,
                t.Propriedade.NmNome, t.Propriedade.NmMunicipio, t.Propriedade.SgEstado))
            .ToListAsync();
    }

    private static TalhaoResponse MapToResponse(Models.Talhao t) =>
        new(t.IdTalhao, t.IdPropriedade, t.NmNome, t.DsCultura,
            t.NrAreaHa, t.FlAtivo, t.DtCriacao,
            t.Propriedade.NmNome, t.Propriedade.NmMunicipio, t.Propriedade.SgEstado);
}
