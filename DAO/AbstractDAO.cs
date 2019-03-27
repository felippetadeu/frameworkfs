using Framework.ClassManipulations;
using Framework.CustomException;
using Framework.DbConnection;
using Framework.Enum;
using Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Framework.DAO
{
    public abstract class AbstractDAO<T> where T : new()
    {
        protected ConnectionFactory ConnectionFactory { get; set; }

        protected int? EmpresaId { get; set; } = null;

        public AbstractDAO(ConnectionFactory connectionFactory, int? empresaId)
        {
            ConnectionFactory = connectionFactory;
            EmpresaId = empresaId;
        }

        public T IdentityInsert(T model)
        {
            List<string> columns = new List<string>();
            foreach (var item in ClassManipulation.GetColumns<T>())
            {
                columns.Add(item);
            }

            string sql = $" Insert Into {ClassManipulation.GetTableName<T>()} (";
            for (var i = 0; i < columns.Count; i++)
            {
                sql += columns[i];
                if (i + 1 != columns.Count) sql += ",";
            }
            sql += ") values (";
            for (var i = 0; i < columns.Count; i++)
            {
                sql += string.Concat(ConnectionFactory.SqlParameter, columns[i]);
                if (i + 1 != columns.Count) sql += ",";
            }
            sql += ")";

            IConnection conn = ConnectionFactory.GetConnection();

            var command = conn.Db.CreateCommand();

            foreach (var item in columns)
            {
                var param = command.CreateParameter();
                param.Value = typeof(T).GetProperty(item).GetValue(model);
                param.ParameterName = item;
                command.Parameters.Add(param);
            }
            command.CommandText = sql;

            var identityProp = typeof(T).GetProperty(ClassManipulation.GetIdentityColumn<T>());
            command.ExecuteNonQuery();
            int pkValue = 0;
            if (ConnectionFactory.ConnectionType == ConnectionEnum.MySql)
            {
                var cmmd = conn.Db.CreateCommand();
                command.CommandText = "SELECT LAST_INSERT_ID()";
            }
            else if (ConnectionFactory.ConnectionType == ConnectionEnum.SqlServer)
            {
                var cmmd = conn.Db.CreateCommand();
                command.CommandText = "SELECT SCOPE_IDENTITY()";
            }

            using (IDataReader reader = command.ExecuteReader())
            {

                if (reader != null && reader.Read())
                    pkValue = Convert.ToInt32(reader.GetInt64(0));

                identityProp.SetValue(model, pkValue);
                return model;
            }
        }

        public T Insert(T model)
        {
            List<string> columns = new List<string>();
            foreach (var item in ClassManipulation.GetColumns<T>())
            {
                columns.Add(item);
            }

            string sql = $" Insert Into {ClassManipulation.GetTableName<T>()} (";
            for (var i = 0; i < columns.Count; i++)
            {
                sql += columns[i];
                if (i + 1 != columns.Count) sql += ",";
            }
            sql += ") values (";
            for (var i = 0; i < columns.Count; i++)
            {
                sql += string.Concat(ConnectionFactory.SqlParameter, columns[i]);
                if (i + 1 != columns.Count) sql += ",";
            }
            sql += ")";

            IConnection conn = ConnectionFactory.GetConnection();

            var command = conn.Db.CreateCommand();
            command.CommandText = sql;

            foreach (var item in columns)
            {
                var param = command.CreateParameter();
                param.Value = typeof(T).GetProperty(item).GetValue(model);
                param.ParameterName = item;
                command.Parameters.Add(param);
            }

            using (IDataReader reader = command.ExecuteReader())
            {
                return model;
            }
        }

        public void Delete(T model)
        {
            string identityColumn = ClassManipulation.GetIdentityColumn<T>();
            string sql = $@" Delete From {ClassManipulation.GetTableName<T>()} 
                              Where {identityColumn} = {string.Concat(ConnectionFactory.SqlParameter, identityColumn)}";

            IConnection conn = ConnectionFactory.GetConnection();

            var command = conn.Db.CreateCommand();

            var param = command.CreateParameter();
            param.Value = typeof(T).GetProperty(identityColumn).GetValue(model);
            param.ParameterName = identityColumn;
            command.Parameters.Add(param);
            command.CommandText = sql;

            command.ExecuteNonQuery();
        }

        public T Update(T model)
        {
            List<string> columns = new List<string>();
            string identityColumn = ClassManipulation.GetIdentityColumn<T>();
            foreach (var item in ClassManipulation.GetColumns<T>())
            {
                columns.Add(item);
            }

            string sql = $" Update {ClassManipulation.GetTableName<T>()} Set ";
            for (var i = 0; i < columns.Count; i++)
            {
                sql += columns[i] + " = " + string.Concat(ConnectionFactory.SqlParameter, columns[i]);
                if (i + 1 != columns.Count) sql += ",";
            }
            sql += $" Where {identityColumn} = {string.Concat(ConnectionFactory.SqlParameter, identityColumn)}";

            IConnection conn = ConnectionFactory.GetConnection();

            var command = conn.Db.CreateCommand();

            columns.Add(identityColumn);
            foreach (var item in columns)
            {
                var param = command.CreateParameter();
                param.Value = typeof(T).GetProperty(item).GetValue(model);
                param.ParameterName = item;
                command.Parameters.Add(param);
            }
            command.CommandText = sql;
            command.ExecuteNonQuery();
            return model;
        }

        public T Find(int identity)
        {
            string identityColumn = ClassManipulation.GetIdentityColumn<T>();
            string sql = $@"
                Select * 
                  From {ClassManipulation.GetTableName<T>()}
                 Where {identityColumn} = {string.Concat(ConnectionFactory.SqlParameter, identityColumn)}";

            IConnection conn = ConnectionFactory.GetConnection();

            var command = conn.Db.CreateCommand();

            var param = command.CreateParameter();
            param.Value = identity;
            param.ParameterName = identityColumn;
            command.Parameters.Add(param);
            command.CommandText = sql;

            using (IDataReader reader = command.ExecuteReader())
            {
                return DataReaderManipulation.DataReaderToClass<T>(reader);
            }
        }

        public T Deactivate(T model)
        {
            var deactivateObject = model as IActivableObject;
            if (deactivateObject != null)
            {
                var id = ClassManipulation.GetIdentityProp<T>();
                if (id != null)
                {
                    model = Find(Convert.ToInt32(id.GetValue(model)));
                    if (model != null)
                    {
                        (model as IActivableObject).Ativo = 0;
                        return Update(model);
                    }
                }
            }
            return default(T);
        }

        public T Activate(T model)
        {
            var activateObject = model as IActivableObject;
            if (activateObject != null)
            {
                var id = ClassManipulation.GetIdentityProp<T>();
                if (id != null)
                {
                    model = Find(Convert.ToInt32(id.GetValue(model)));
                    if (model != null)
                    {
                        (model as IActivableObject).Ativo = 1;
                        return Update(model);
                    }
                }
            }
            return default(T);
        }

        public List<T> List()
        {
            string sql = $@"
                Select * 
                  From {ClassManipulation.GetTableName<T>()}";

            if (typeof(T) is IChildEmpresaObject)
            {
                if (!EmpresaId.HasValue)
                    throw new BrokenRulesException("Não foi informado o código da empresa logada");
                sql += $" Where EmpresaId == {EmpresaId.Value}";
            }

            IConnection conn = ConnectionFactory.GetConnection();

            var command = conn.Db.CreateCommand();
            command.CommandText = sql;
            DbDataReader reader = command.ExecuteReader();

            return DataReaderManipulation.DataReaderMapToList<T>(reader);
        }

        public List<T> List(FilterObject<T> filter)
        {
            var model = filter.Model;
            string sql = $@"
                Select * 
                  From {ClassManipulation.GetTableName<T>()}";

            IConnection conn = ConnectionFactory.GetConnection();

            var command = conn.Db.CreateCommand();

            if (model != null && filter != null && filter.Properties.Count > 0)
            {
                sql += " Where ";
                for (var i = 0; i < filter.Properties.Count; i++)
                {
                    var prop = filter.Properties[i];

                    sql += prop.LogicalOperator;

                    var property = ClassManipulation.GetColumn<T>(prop.Property);
                    sql += property.Name + " " 
                            + prop.Operator + " " 
                            + ConnectionFactory.SqlParameter + property.Name;

                    var param = command.CreateParameter();
                    param.ParameterName = property.Name;

                    if (prop.OperatorEnum == OperatorEnum.Like)
                        param.Value = prop.PreAppend + property.GetValue(model) + prop.PosAppend;
                    else
                        param.Value = property.GetValue(model);

                    command.Parameters.Add(param);
                }
                if (typeof(T) is IChildEmpresaObject)
                {
                    if (!EmpresaId.HasValue)
                        throw new BrokenRulesException("Não foi informado o código da empresa logada");
                    sql += $" And EmpresaId == {EmpresaId.Value}";
                }
            }
            else
            {
                if (typeof(T) is IChildEmpresaObject)
                {
                    if (!EmpresaId.HasValue)
                        throw new BrokenRulesException("Não foi informado o código da empresa logada");
                    sql += $" Where EmpresaId == {EmpresaId.Value}";
                }
            }

            command.CommandText = sql;

            using (DbDataReader reader = command.ExecuteReader())
                return DataReaderManipulation.DataReaderMapToList<T>(reader);
        }
    }
}
