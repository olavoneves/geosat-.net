using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_CONFIGURACAO")]
public class Configuracao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_CONFIG")]
    public int IdConfig { get; set; }

    [Column("ID_TALHAO")]
    public int IdTalhao { get; set; }

    [Column("NR_THRESHOLD_UMID_MIN")]
    public decimal NrThresholdUmidMin { get; set; } = 30;

    [Column("NR_THRESHOLD_NDVI_MIN")]
    public decimal NrThresholdNdviMin { get; set; } = (decimal)0.3;

    [Column("NR_JANELA_FUSAO_HORAS")]
    public int NrJanelaFusaoHoras { get; set; } = 48;

    [Column("DT_ATUALIZACAO")]
    public DateTime DtAtualizacao { get; set; }

    public Talhao Talhao { get; set; } = null!;
}
