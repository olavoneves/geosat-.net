using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_USUARIO_NET")]
public class UsuarioNet
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Required]
    [Column("NM_NOME")]
    public string NmNome { get; set; } = string.Empty;

    [Required]
    [Column("DS_EMAIL")]
    public string DsEmail { get; set; } = string.Empty;

    [Required]
    [Column("DS_SENHA_HASH")]
    public string DsSenhaHash { get; set; } = string.Empty;

    [Column("DS_ROLE")]
    public string DsRole { get; set; } = "USER";

    [Column("FL_ATIVO")]
    public string FlAtivo { get; set; } = "S";

    [Column("DT_CRIACAO")]
    public DateTime DtCriacao { get; set; } = DateTime.UtcNow;
}
