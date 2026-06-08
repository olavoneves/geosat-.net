namespace GeoSat.API.DTOs.Response;

public record DashboardResponse(
    int TotalProdutores,
    int TotalPropriedades,
    int TotalTalhoes,
    int AlertasPendentes,
    int AlertasCriticos,
    int ImagensPendentes
);

public record AlertasPorNivelResponse(
    int Atencao,
    int Alerta,
    int Critico
);

public record TalhaoEmRiscoResponse(
    int IdTalhao,
    string NmTalhao,
    string DsCultura,
    string NmPropriedade,
    string NmMunicipio,
    string SgEstado,
    int TotalAlertas,
    string PiorNivel
);

public record NdviMedioResponse(
    int IdTalhao,
    string NmTalhao,
    decimal? NdviMedio,
    int TotalImagens,
    DateTime? UltimaCaptura
);
