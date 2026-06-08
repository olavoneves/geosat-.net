using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSat.API.Models;

[Table("TB_GST_REFRESH_TOKEN_NET")]
public class RefreshTokenNet
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_REFRESH")]
    public int IdRefresh { get; set; }

    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Required]
    [Column("DS_TOKEN")]
    public string DsToken { get; set; } = string.Empty;

    [Column("DT_EXPIRACAO")]
    public DateTime DtExpiracao { get; set; }

    [Column("FL_REVOGADO")]
    public string FlRevogado { get; set; } = "N";

    [Column("DT_CRIACAO")]
    public DateTime DtCriacao { get; set; } = DateTime.UtcNow;

    public UsuarioNet Usuario { get; set; } = null!;
}
