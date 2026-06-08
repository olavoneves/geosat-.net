using System.ComponentModel.DataAnnotations;

namespace GeoSat.API.DTOs.Request;

public record UsuarioRequest(
    [Required][MinLength(3)][MaxLength(100)] string NmNome,
    [Required][EmailAddress][MaxLength(150)] string DsEmail,
    [Required][MinLength(6)][MaxLength(100)] string DsSenha,
    [Required][RegularExpression("^(ADMIN|USER)$")] string DsRole
);

public record UsuarioUpdateRequest(
    [Required][MinLength(3)][MaxLength(100)] string NmNome,
    [Required][RegularExpression("^(ADMIN|USER)$")] string DsRole
);
