using System.ComponentModel.DataAnnotations;

namespace GeoSat.API.DTOs.Request;

public record ImagemSatelitalRequest(
    [Required] int IdTalhao,
    [Required] DateTime DtCaptura,
    [Required][RegularExpression("^(NASA|ESA)$")] string DsFonte
);

public record ProcessarImagemRequest(
    [Required][Range(-1.0, 1.0)] decimal NrNdvi
);

public record ErroImagemRequest(
    [Required][MaxLength(500)] string DsErro
);
