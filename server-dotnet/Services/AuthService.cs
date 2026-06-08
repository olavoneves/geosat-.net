using GeoSat.API.Data;
using GeoSat.API.DTOs.Request;
using GeoSat.API.DTOs.Response;
using GeoSat.API.Middleware;
using GeoSat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await _db.UsuariosNet
            .FirstOrDefaultAsync(u => u.DsEmail == request.Email && u.FlAtivo == "S")
            ?? throw new UnauthorizedAccessException("Credenciais inválidas");

        if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.DsSenhaHash))
            throw new UnauthorizedAccessException("Credenciais inválidas");

        return await GerarTokensAsync(usuario);
    }

    public async Task<TokenResponse> RefreshAsync(RefreshTokenRequest request)
    {
        var tokenHash = AuthMiddleware.HashToken(request.RefreshToken);

        var tokenRecord = await _db.RefreshTokensNet
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r =>
                r.DsToken == tokenHash &&
                r.FlRevogado == "N" &&
                r.DtExpiracao > DateTime.UtcNow)
            ?? throw new UnauthorizedAccessException("Refresh token inválido ou expirado");

        // revoga o token atual
        tokenRecord.FlRevogado = "S";
        await _db.SaveChangesAsync();

        return await GerarTokensAsync(tokenRecord.Usuario);
    }

    public async Task LogoutAsync(UsuarioNet usuario)
    {
        var tokens = await _db.RefreshTokensNet
            .Where(r => r.IdUsuario == usuario.IdUsuario && r.FlRevogado == "N")
            .ToListAsync();

        foreach (var t in tokens)
            t.FlRevogado = "S";

        await _db.SaveChangesAsync();
    }

    public async Task<UsuarioResponse> RegisterAsync(UsuarioRequest request)
    {
        var existe = await _db.UsuariosNet.AnyAsync(u => u.DsEmail == request.DsEmail);
        if (existe)
            throw new InvalidOperationException("E-mail já cadastrado");

        var usuario = new UsuarioNet
        {
            NmNome = request.NmNome,
            DsEmail = request.DsEmail,
            DsSenhaHash = BCrypt.Net.BCrypt.HashPassword(request.DsSenha),
            DsRole = request.DsRole,
            FlAtivo = "S",
            DtCriacao = DateTime.UtcNow
        };

        _db.UsuariosNet.Add(usuario);
        await _db.SaveChangesAsync();

        return MapToResponse(usuario);
    }

    private async Task<TokenResponse> GerarTokensAsync(UsuarioNet usuario)
    {
        var accessTokenMinutes = _config.GetValue<int>("Auth:AccessTokenExpirationMinutes", 30);
        var refreshTokenDays = _config.GetValue<int>("Auth:RefreshTokenExpirationDays", 7);

        var accessToken = Guid.NewGuid().ToString("N");
        var refreshToken = Guid.NewGuid().ToString("N");

        var accessRecord = new RefreshTokenNet
        {
            IdUsuario = usuario.IdUsuario,
            DsToken = AuthMiddleware.HashToken(accessToken),
            DtExpiracao = DateTime.UtcNow.AddMinutes(accessTokenMinutes),
            FlRevogado = "N",
            DtCriacao = DateTime.UtcNow
        };

        var refreshRecord = new RefreshTokenNet
        {
            IdUsuario = usuario.IdUsuario,
            DsToken = AuthMiddleware.HashToken(refreshToken),
            DtExpiracao = DateTime.UtcNow.AddDays(refreshTokenDays),
            FlRevogado = "N",
            DtCriacao = DateTime.UtcNow
        };

        _db.RefreshTokensNet.AddRange(accessRecord, refreshRecord);
        await _db.SaveChangesAsync();

        return new TokenResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresIn: accessTokenMinutes * 60,
            Role: usuario.DsRole
        );
    }

    private static UsuarioResponse MapToResponse(UsuarioNet u) =>
        new(u.IdUsuario, u.NmNome, u.DsEmail, u.DsRole, u.FlAtivo, u.DtCriacao);
}
