using System.ComponentModel.DataAnnotations;

namespace GeoSat.API.DTOs.Request;

public record RefreshTokenRequest(
    [Required] string RefreshToken
);
