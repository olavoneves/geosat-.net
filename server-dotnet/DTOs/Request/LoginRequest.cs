using System.ComponentModel.DataAnnotations;

namespace GeoSat.API.DTOs.Request;

public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Senha
);
