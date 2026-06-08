namespace GeoSat.API.DTOs.Response;

public record UsuarioResponse(
    int IdUsuario,
    string NmNome,
    string DsEmail,
    string DsRole,
    string FlAtivo,
    DateTime DtCriacao
);
