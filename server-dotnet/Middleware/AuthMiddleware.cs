using GeoSat.API.Data;
using GeoSat.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GeoSat.API.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    private static readonly string[] PublicRoutes =
    [
        "/auth/login", "/auth/refresh",
        "/swagger", "/v3/api-docs", "/favicon.ico"
    ];

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        if (PublicRoutes.Any(r => path.StartsWith(r)))
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Token não fornecido" });
            return;
        }

        var token = authHeader[7..];
        var tokenHash = HashToken(token);

        var tokenRecord = await db.RefreshTokensNet
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r =>
                r.DsToken == tokenHash &&
                r.FlRevogado == "N" &&
                r.DtExpiracao > DateTime.UtcNow);

        if (tokenRecord == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Token inválido ou expirado" });
            return;
        }

        if (tokenRecord.Usuario.FlAtivo != "S")
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Usuário inativo" });
            return;
        }

        context.Items["Usuario"] = tokenRecord.Usuario;
        await _next(context);
    }

    public static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes).ToLower();
    }
}
