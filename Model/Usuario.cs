using Framework.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Model
{
    [Table("usuario")]
    public class Usuario : IActivableObject
    {
        [Key]
        public int UsuarioId { get; set; }

        [Column("EmpresaId")]
        public int EmpresaId { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Senha")]
        public string Senha { get; set; }

        [Column("Token")]
        public string Token { get; set; }

        [Column("Administrativo")]
        public int Administrativo { get; set; }

        [Column("Ativo")]
        public int Ativo { get; set; }

        public bool IsAtivo
        {
            get
            {
                return Ativo == 1;
            }
        }
    }
}
