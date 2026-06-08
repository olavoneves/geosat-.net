namespace GeoSat.API.DTOs.Response;

public record TalhaoResponse(
    int IdTalhao,
    int IdPropriedade,
    string NmNome,
    string DsCultura,
    decimal NrAreaHa,
    string FlAtivo,
    DateTime DtCriacao,
    string NmPropriedade,
    string NmMunicipio,
    string SgEstado
);
