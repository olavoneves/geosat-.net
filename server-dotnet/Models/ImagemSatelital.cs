using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_IMAGEM_SATELITAL")]
public class ImagemSatelital
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_IMAGEM")]
    public int IdImagem { get; set; }

    [Column("ID_TALHAO")]
    public int IdTalhao { get; set; }

    [Column("DT_CAPTURA")]
    public DateTime DtCaptura { get; set; }

    [Column("NR_NDVI")]
    public decimal? NrNdvi { get; set; }

    [Required]
    [Column("DS_FONTE")]
    public string DsFonte { get; set; } = string.Empty;

    [Column("DS_STATUS_PROC")]
    public string DsStatusProc { get; set; } = "PENDENTE";

    [Column("DS_ERRO")]
    public string? DsErro { get; set; }

    [Column("DT_PROCESSADO")]
    public DateTime? DtProcessado { get; set; }

    public Talhao Talhao { get; set; } = null!;
}
