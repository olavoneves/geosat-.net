namespace GeoSat.API.DTOs.Response;

public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    string Role
);
