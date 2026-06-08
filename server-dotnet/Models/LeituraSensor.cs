using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_LEITURA_SENSOR")]
public class LeituraSensor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_LEITURA")]
    public int IdLeitura { get; set; }

    [Column("ID_SENSOR")]
    public int IdSensor { get; set; }

    [Column("DT_LEITURA")]
    public DateTime DtLeitura { get; set; }

    [Column("NR_TEMP_AR")]
    public decimal NrTempAr { get; set; }

    [Column("NR_UMIDADE_SOLO")]
    public decimal NrUmidadeSolo { get; set; }

    [Column("NR_LUMINOSIDADE")]
    public decimal? NrLuminosidade { get; set; }

    [Column("FL_TRANSMITIDA")]
    public string FlTransmitida { get; set; } = "N";

    [Column("DT_RECEBIDA")]
    public DateTime DtRecebida { get; set; }

    public Sensor Sensor { get; set; } = null!;
}
