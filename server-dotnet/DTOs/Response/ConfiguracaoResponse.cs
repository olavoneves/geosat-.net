namespace GeoSat.API.DTOs.Response;

public record ConfiguracaoResponse(
    int IdConfig,
    int IdTalhao,
    string NmTalhao,
    decimal NrThresholdUmidMin,
    decimal NrThresholdNdviMin,
    int NrJanelaFusaoHoras,
    DateTime DtAtualizacao
);
