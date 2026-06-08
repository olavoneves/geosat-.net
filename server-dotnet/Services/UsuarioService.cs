using GeoSat.API.Data;
using GeoSat.API.DTOs.Request;
using GeoSat.API.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Services;

public class UsuarioService
{
    private readonly AppDbContext _db;

    public UsuarioService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UsuarioResponse>> ListarAsync()
    {
        return await _db.UsuariosNet
            .Where(u => u.FlAtivo == "S")
            .OrderBy(u => u.NmNome)
            .Select(u => new UsuarioResponse(u.IdUsuario, u.NmNome, u.DsEmail, u.DsRole, u.FlAtivo, u.DtCriacao))
            .ToListAsync();
    }

    public async Task<UsuarioResponse> BuscarPorIdAsync(int id)
    {
        var u = await _db.UsuariosNet.FindAsync(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado");

        return new UsuarioResponse(u.IdUsuario, u.NmNome, u.DsEmail, u.DsRole, u.FlAtivo, u.DtCriacao);
    }

    public async Task<UsuarioResponse> AtualizarAsync(int id, UsuarioUpdateRequest request)
    {
        var u = await _db.UsuariosNet.FindAsync(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado");

        u.NmNome = request.NmNome;
        u.DsRole = request.DsRole;
        await _db.SaveChangesAsync();

        return new UsuarioResponse(u.IdUsuario, u.NmNome, u.DsEmail, u.DsRole, u.FlAtivo, u.DtCriacao);
    }

    public async Task DesativarAsync(int id)
    {
        var u = await _db.UsuariosNet.FindAsync(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado");

        u.FlAtivo = "N";
        await _db.SaveChangesAsync();
    }
}
