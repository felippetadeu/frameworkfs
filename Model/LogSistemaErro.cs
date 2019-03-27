using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Model
{
    [Table("logsistemaerro")]
    public class LogSistemaErro
    {
        [Key]
        public int LogSistemaErroId { get; set; }

        [Column("UsuarioId")]
        public int UsuarioId { get; set; }

        [Column("Mensagem")]
        public string Mensagem { get; set; }

        [Column("Erro")]
        public string Erro { get; set; }

        [Column("DataHora")]
        public DateTime DataHora { get; set; }

        [Column("Resolvido")]
        public int Resolvido { get; set; }
    }
}
