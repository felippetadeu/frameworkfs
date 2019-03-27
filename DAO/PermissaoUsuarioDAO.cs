using Framework.ClassManipulations;
using Framework.DbConnection;
using Framework.Interfaces;
using Framework.Model;
using System.Data.Common;

namespace Framework.DAO
{
    public class PermissaoUsuarioDAO : AbstractDAO<PermissaoUsuario>
    {
        public PermissaoUsuarioDAO(ConnectionFactory connectionFactory, int? empresaId) : base(connectionFactory, empresaId)
        {
        }

        public bool UsuarioPossuiPermissao(int usuarioId, string controllerName, string actionName)
        {
            IConnection conn = ConnectionFactory.GetConnection();

            string sql = $@"
                 Select *
                   From Usuario as U
              Left Join PermissaoUsuario as P     
                     On P.UsuarioId = U.UsuarioId
                  Where P.UsuarioId = @usuarioId
                    And Exists (
                      Select 1
                        From Acao A
                        Join Controlador C
                          On A.ControladorId = C.ControladorId
                       Where A.Nome = @actionName
                         And C.Nome = @controllerName
                         And A.AcaoId = P.AcaoId
                    ) Or U.Administrativo = 1
                ";

            var command = conn.Db.CreateCommand();

            var param = command.CreateParameter();
            param.ParameterName = "usuarioId";
            param.Value = usuarioId;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "actionName";
            param.Value = actionName;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "controllerName";
            param.Value = controllerName;
            command.Parameters.Add(param);

            command.CommandText = sql;

            using (DbDataReader reader = command.ExecuteReader())
                return reader.Read();
        }
    }
}
