using Framework.DAO;
using Framework.DbConnection;
using Framework.Model;

namespace Framework.BO
{
    public class LogSistemaBO : AbstractBO<LogSistema>
    {
        public LogSistemaBO(ConnectionFactory factory, int usuarioId, int? empresaId) : base(factory, usuarioId, empresaId)
        {
        }

        public override AbstractDAO<LogSistema> GetDAO()
        {
            if (DAO == null)
            {
                DAO = new LogSistemaDAO(ConnectionFactory, EmpresaId);
            }

            return DAO;
        }

        public LogSistemaDAO UsuarioDAO
        {
            get
            {
                return (LogSistemaDAO)GetDAO();
            }
        }
    }
}
