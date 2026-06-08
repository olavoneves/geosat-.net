using GeoSat.API.Data;
using GeoSat.API.DTOs.Request;
using GeoSat.API.DTOs.Response;
using GeoSat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Services;

public class ImagemSatelitalService
{
    private readonly AppDbContext _db;

    public ImagemSatelitalService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ImagemSatelitalResponse> RegistrarAsync(ImagemSatelitalRequest request)
    {
        var talhao = await _db.Talhoes.FindAsync(request.IdTalhao)
            ?? throw new KeyNotFoundException($"Talhão {request.IdTalhao} não encontrado");

        var imagem = new ImagemSatelital
        {
            IdTalhao = request.IdTalhao,
            DtCaptura = request.DtCaptura,
            DsFonte = request.DsFonte,
            DsStatusProc = "PENDENTE"
        };

        _db.ImagensSatelitais.Add(imagem);
        await _db.SaveChangesAsync();

        return MapToResponse(imagem, talhao.NmNome);
    }

    public async Task<ImagemSatelitalResponse> BuscarPorIdAsync(int id)
    {
        var imagem = await _db.ImagensSatelitais
            .Include(i => i.Talhao)
            .FirstOrDefaultAsync(i => i.IdImagem == id)
            ?? throw new KeyNotFoundException($"Imagem {id} não encontrada");

        return MapToResponse(imagem, imagem.Talhao.NmNome);
    }

    public async Task<(IEnumerable<ImagemSatelitalResponse> Items, int Total)> ListarPorTalhaoAsync(
        int idTalhao, int page, int size)
    {
        var talhao = await _db.Talhoes.FindAsync(idTalhao)
            ?? throw new KeyNotFoundException($"Talhão {idTalhao} não encontrado");

        var query = _db.ImagensSatelitais.Where(i => i.IdTalhao == idTalhao);
        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.DtCaptura)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (items.Select(i => MapToResponse(i, talhao.NmNome)), total);
    }

    public async Task<IEnumerable<ImagemSatelitalResponse>> ListarPendentesAsync()
    {
        return await _db.ImagensSatelitais
            .Include(i => i.Talhao)
            .Where(i => i.DsStatusProc == "PENDENTE")
            .OrderBy(i => i.DtCaptura)
            .Select(i => new ImagemSatelitalResponse(
                i.IdImagem, i.IdTalhao, i.Talhao.NmNome, i.DtCaptura,
                i.NrNdvi, i.DsFonte, i.DsStatusProc, i.DsErro, i.DtProcessado))
            .ToListAsync();
    }

    public async Task<ImagemSatelitalResponse> ProcessarAsync(int id, ProcessarImagemRequest request)
    {
        var imagem = await _db.ImagensSatelitais
            .Include(i => i.Talhao)
            .FirstOrDefaultAsync(i => i.IdImagem == id)
            ?? throw new KeyNotFoundException($"Imagem {id} não encontrada");

        if (imagem.DsStatusProc != "PENDENTE")
            throw new InvalidOperationException("Apenas imagens pendentes podem ser processadas");

        imagem.NrNdvi = request.NrNdvi;
        imagem.DsStatusProc = "PROCESSADO";
        imagem.DtProcessado = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return MapToResponse(imagem, imagem.Talhao.NmNome);
    }

    public async Task<ImagemSatelitalResponse> RegistrarErroAsync(int id, ErroImagemRequest request)
    {
        var imagem = await _db.ImagensSatelitais
            .Include(i => i.Talhao)
            .FirstOrDefaultAsync(i => i.IdImagem == id)
            ?? throw new KeyNotFoundException($"Imagem {id} não encontrada");

        imagem.DsStatusProc = "ERRO";
        imagem.DsErro = request.DsErro;
        await _db.SaveChangesAsync();

        return MapToResponse(imagem, imagem.Talhao.NmNome);
    }

    private static ImagemSatelitalResponse MapToResponse(ImagemSatelital i, string nmTalhao) =>
        new(i.IdImagem, i.IdTalhao, nmTalhao, i.DtCaptura,
            i.NrNdvi, i.DsFonte, i.DsStatusProc, i.DsErro, i.DtProcessado);
}
