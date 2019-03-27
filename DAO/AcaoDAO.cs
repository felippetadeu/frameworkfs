using Framework.DbConnection;
using Framework.Model;

namespace Framework.DAO
{
    public class AcaoDAO : AbstractDAO<Acao>
    {
        public AcaoDAO(ConnectionFactory connectionFactory, int? empresaId) : base(connectionFactory, empresaId)
        {
        }
    }
}
