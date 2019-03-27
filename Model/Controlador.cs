using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Model
{
    [Table("controlador")]
    public class Controlador
    {
        [Key]
        public int ControladorId { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }

        [Column("NomeTela")]
        public string NomeTela { get; set; }
    }
}
