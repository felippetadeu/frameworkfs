using Framework.DbConnection;
using Framework.Model;

namespace Framework.DAO
{
    public class LogSistemaDAO : AbstractDAO<LogSistema>
    {
        public LogSistemaDAO(ConnectionFactory connectionFactory, int? empresaId) : base(connectionFactory, empresaId)
        {
        }
    }
}
