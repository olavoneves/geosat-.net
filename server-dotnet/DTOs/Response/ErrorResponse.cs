namespace GeoSat.API.DTOs.Response;

public record ErrorResponse(
    int Status,
    string Error,
    string Message,
    string Path,
    DateTime Timestamp
);
