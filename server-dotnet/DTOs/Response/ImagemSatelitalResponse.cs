namespace GeoSat.API.DTOs.Response;

public record ImagemSatelitalResponse(
    int IdImagem,
    int IdTalhao,
    string NmTalhao,
    DateTime DtCaptura,
    decimal? NrNdvi,
    string DsFonte,
    string DsStatusProc,
    string? DsErro,
    DateTime? DtProcessado
);
