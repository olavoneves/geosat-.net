using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_TALHAO")]
public class Talhao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_TALHAO")]
    public int IdTalhao { get; set; }

    [Column("ID_PROPRIEDADE")]
    public int IdPropriedade { get; set; }

    [Required]
    [Column("NM_NOME")]
    public string NmNome { get; set; } = string.Empty;

    [Required]
    [Column("DS_CULTURA")]
    public string DsCultura { get; set; } = string.Empty;

    [Column("NR_AREA_HA")]
    public decimal NrAreaHa { get; set; }

    [Column("FL_ATIVO")]
    public string FlAtivo { get; set; } = "S";

    [Column("DT_CRIACAO")]
    public DateTime DtCriacao { get; set; }

    public Propriedade Propriedade { get; set; } = null!;
    public ICollection<Sensor> Sensores { get; set; } = new List<Sensor>();
    public ICollection<ImagemSatelital> ImagensSatelitais { get; set; } = new List<ImagemSatelital>();
    public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
    public Configuracao? Configuracao { get; set; }
}
