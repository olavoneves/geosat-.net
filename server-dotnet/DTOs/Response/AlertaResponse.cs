namespace GeoSat.API.DTOs.Response;

public record AlertaResponse(
    int IdAlerta,
    int IdTalhao,
    string NmTalhao,
    string TpTipo,
    string TpNivel,
    string TpOrigem,
    string DsDescricao,
    string StStatus,
    DateTime DtGerado,
    DateTime? DtVisualizado,
    DateTime? DtResolvido
);

public record AlertaDetalheResponse(
    int IdAlerta,
    int IdTalhao,
    string NmTalhao,
    string TpTipo,
    string TpNivel,
    string TpOrigem,
    string DsDescricao,
    string StStatus,
    DateTime DtGerado,
    DateTime? DtVisualizado,
    DateTime? DtResolvido,
    IEnumerable<LogAlertaResponse> Logs
);

public record LogAlertaResponse(
    int IdLog,
    string DsAcao,
    string DsOrigem,
    string? DsObservacao,
    DateTime DtEvento
);
