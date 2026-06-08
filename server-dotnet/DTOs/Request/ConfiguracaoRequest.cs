using System.ComponentModel.DataAnnotations;

namespace GeoSat.API.DTOs.Request;

public record ConfiguracaoRequest(
    [Required][Range(0, 100)] decimal NrThresholdUmidMin,
    [Required][Range(-1.0, 1.0)] decimal NrThresholdNdviMin,
    [Required][Range(1, 720)] int NrJanelaFusaoHoras
);
