using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_SENSOR")]
public class Sensor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_SENSOR")]
    public int IdSensor { get; set; }

    [Column("ID_TALHAO")]
    public int IdTalhao { get; set; }

    [Required]
    [Column("CD_IDENTIFICADOR_HW")]
    public string CdIdentificadorHw { get; set; } = string.Empty;

    [Column("DS_LOCALIZACAO")]
    public string? DsLocalizacao { get; set; }

    [Column("FL_ATIVO")]
    public string FlAtivo { get; set; } = "S";

    [Column("DT_INSTALACAO")]
    public DateTime DtInstalacao { get; set; }

    public Talhao Talhao { get; set; } = null!;
    public ICollection<LeituraSensor> Leituras { get; set; } = new List<LeituraSensor>();
}
