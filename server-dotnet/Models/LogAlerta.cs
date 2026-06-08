using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_LOG_ALERTA")]
public class LogAlerta
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_LOG")]
    public int IdLog { get; set; }

    [Column("ID_ALERTA")]
    public int IdAlerta { get; set; }

    [Required]
    [Column("DS_ACAO")]
    public string DsAcao { get; set; } = string.Empty;

    [Required]
    [Column("DS_ORIGEM")]
    public string DsOrigem { get; set; } = string.Empty;

    [Column("DS_OBSERVACAO")]
    public string? DsObservacao { get; set; }

    [Column("DT_EVENTO")]
    public DateTime DtEvento { get; set; }

    public Alerta Alerta { get; set; } = null!;
}
