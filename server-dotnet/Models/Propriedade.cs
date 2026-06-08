using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_PROPRIEDADE")]
public class Propriedade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_PROPRIEDADE")]
    public int IdPropriedade { get; set; }

    [Column("ID_PRODUTOR")]
    public int IdProdutor { get; set; }

    [Required]
    [Column("NM_NOME")]
    public string NmNome { get; set; } = string.Empty;

    [Required]
    [Column("NM_MUNICIPIO")]
    public string NmMunicipio { get; set; } = string.Empty;

    [Required]
    [Column("SG_ESTADO")]
    public string SgEstado { get; set; } = string.Empty;

    [Column("NR_AREA_HA")]
    public decimal NrAreaHa { get; set; }

    [Column("FL_ATIVA")]
    public string FlAtiva { get; set; } = "S";

    [Column("DT_CRIACAO")]
    public DateTime DtCriacao { get; set; }

    public Produtor Produtor { get; set; } = null!;
    public ICollection<Talhao> Talhoes { get; set; } = new List<Talhao>();
}
