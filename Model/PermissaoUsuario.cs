using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Model
{
    [Table("permissaousuario")]
    public class PermissaoUsuario
    {
        [Key]
        public int PermissaoUsuarioId { get; set; }
        [Column("UsuarioId")]
        public int UsuarioId { get; set; }
        [Column("AcaoId")]
        public int AcaoId { get; set; }
    }
}
