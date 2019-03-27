using Framework.ClassManipulations;
using Framework.DbConnection;
using Framework.Interfaces;
using Framework.Model;
using System.Data.Common;

namespace Framework.DAO
{
    public class UsuarioDAO : AbstractDAO<Usuario>
    {
        public UsuarioDAO(ConnectionFactory connectionFactory, int? empresaId = null) : base(connectionFactory, empresaId)
        {
        }

        public Usuario Login(string user, string pwd, int empresaId)
        {
            IConnection conn = ConnectionFactory.GetConnection();

            string sql = $@"
                Select * 
                  From {ClassManipulation.GetTableName<Usuario>()}
                 Where Login = {conn.SqlParameter}user
                   And Senha = {conn.SqlParameter}pwd
                   And Ativo = 1
                   And EmpresaID = {empresaId}
                ";

            var command = conn.Db.CreateCommand();

            var param = command.CreateParameter();
            param.ParameterName = "user";
            param.Value = user;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "pwd";
            param.Value = pwd;
            command.Parameters.Add(param);

            command.CommandText = sql;

            using (DbDataReader reader = command.ExecuteReader())
                return DataReaderManipulation.DataReaderToClass<Usuario>(reader);
        }

        public bool ValidaEmailExistente(string email, int id)
        {
            IConnection conn = ConnectionFactory.GetConnection();

            string sql = $@"
                Select * 
                  From {ClassManipulation.GetTableName<Usuario>()}
                 Where Email = {conn.SqlParameter}email
                   And EmpresaID = {EmpresaId}
                   And ({id} <> UsuarioId)
                ";

            var command = conn.Db.CreateCommand();

            var param = command.CreateParameter();
            param.ParameterName = "email";
            param.Value = email;

            command.Parameters.Add(param);

            command.CommandText = sql;

            using (DbDataReader reader = command.ExecuteReader())
                return reader.Read();
        }
    }
}
