using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Model
{
    [Table("acao")]
    public class Acao
    {
        [Key]
        public int AcaoId { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }

        [Column("NomeTela")]
        public string NomeTela { get; set; }
    }
}
