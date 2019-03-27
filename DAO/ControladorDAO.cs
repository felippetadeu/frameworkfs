using Framework.DbConnection;
using Framework.Model;

namespace Framework.DAO
{
    public class ControladorDAO : AbstractDAO<Controlador>
    {
        public ControladorDAO(ConnectionFactory connectionFactory, int? empresaId) : base(connectionFactory, empresaId)
        {
        }
    }
}
