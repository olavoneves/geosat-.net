using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_ALERTA")]
public class Alerta
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_ALERTA")]
    public int IdAlerta { get; set; }

    [Column("ID_TALHAO")]
    public int IdTalhao { get; set; }

    [Required]
    [Column("TP_TIPO")]
    public string TpTipo { get; set; } = string.Empty;

    [Required]
    [Column("TP_NIVEL")]
    public string TpNivel { get; set; } = string.Empty;

    [Required]
    [Column("TP_ORIGEM")]
    public string TpOrigem { get; set; } = string.Empty;

    [Required]
    [Column("DS_DESCRICAO")]
    public string DsDescricao { get; set; } = string.Empty;

    [Column("ST_STATUS")]
    public string StStatus { get; set; } = "PENDENTE";

    [Column("DT_GERADO")]
    public DateTime DtGerado { get; set; }

    [Column("DT_VISUALIZADO")]
    public DateTime? DtVisualizado { get; set; }

    [Column("DT_RESOLVIDO")]
    public DateTime? DtResolvido { get; set; }

    public Talhao Talhao { get; set; } = null!;
    public ICollection<LogAlerta> Logs { get; set; } = new List<LogAlerta>();
}
