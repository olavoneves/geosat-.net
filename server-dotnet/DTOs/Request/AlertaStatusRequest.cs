using System.ComponentModel.DataAnnotations;

namespace GeoSat.API.DTOs.Request;

public record AlertaVisualizarRequest();

public record AlertaResolverRequest(
    [MaxLength(500)] string? Observacao
);

public record AlertaReabrirRequest(
    [MaxLength(500)] string? Observacao
);
