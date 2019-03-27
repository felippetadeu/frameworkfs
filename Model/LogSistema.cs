using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Model
{
    [Table("logsistema")]
    public class LogSistema
    {
        [Key]
        public int LogSistemaId { get; set; }
        
        [Column("UsuarioId")]
        public int UsuarioId { get; set; }

        [Column("Acao")]
        public int Acao { get; set; }

        [Column("DataHora")]
        public DateTime DataHora { get; set; } = DateTime.UtcNow;

        [Column("Dados")]
        public string Dados { get; set; }

        [Column("EmpresaId")]
        public int EmpresaId { get; set; }
    }
}
