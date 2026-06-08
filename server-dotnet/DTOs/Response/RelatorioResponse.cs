namespace GeoSat.API.DTOs.Response;

public record RelatorioAlertaResponse(
    int IdTalhao,
    string NmTalhao,
    string DsCultura,
    int TotalAlertas,
    int Pendentes,
    int Visualizados,
    int Resolvidos,
    int Atencao,
    int Alertas,
    int Criticos
);

public record TempoRespostaResponse(
    int IdTalhao,
    string NmTalhao,
    double? TempoMedioVisualizacaoHoras,
    double? TempoMedioResolucaoHoras,
    int TotalAlertasResolvidos
);

public record NdviHistoricoResponse(
    int IdTalhao,
    string NmTalhao,
    IEnumerable<NdviPontoResponse> Historico
);

public record NdviPontoResponse(
    DateTime DtCaptura,
    decimal NrNdvi,
    string DsFonte
);
