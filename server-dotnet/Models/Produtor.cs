using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_PRODUTOR")]
public class Produtor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_PRODUTOR")]
    public int IdProdutor { get; set; }

    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Required]
    [Column("NM_NOME")]
    public string NmNome { get; set; } = string.Empty;

    [Required]
    [Column("NR_CPF")]
    public string NrCpf { get; set; } = string.Empty;

    [Required]
    [Column("DS_EMAIL")]
    public string DsEmail { get; set; } = string.Empty;

    [Column("NR_TELEFONE")]
    public string? NrTelefone { get; set; }

    [Column("FL_ATIVO")]
    public string FlAtivo { get; set; } = "S";

    [Column("DT_CRIACAO")]
    public DateTime DtCriacao { get; set; }

    public ICollection<Propriedade> Propriedades { get; set; } = new List<Propriedade>();
}
