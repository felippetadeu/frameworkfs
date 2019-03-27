using Framework.Enum;
using Framework.Interfaces;
using System;

namespace Framework.DbConnection
{
    public class ConnectionFactory : IDisposable
    {
        private IConnection DbConnection { get; set; }

        public ConnectionEnum ConnectionType { get; private set; }

        private int TransactionCount { get; set; }

        public string SqlParameter
        {
            get
            {
                return DbConnection.SqlParameter;
            }
        }

        public ConnectionFactory(string connectionString, ConnectionEnum connectionType)
        {
            ConnectionType = connectionType;

            switch (connectionType)
            {
                case ConnectionEnum.MySql:
                    DbConnection = new MySQLConnection(connectionString);
                    break;
                case ConnectionEnum.SqlServer:
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {
            if (DbConnection.IsOpen())
                TransactionCount = 0;

            DbConnection.Dispose();
        }

        public IConnection GetConnection()
        {
            if (!DbConnection.IsOpen())
                DbConnection.Open();

            return DbConnection;
        }

        public void BeginTransaction()
        {
            TransactionCount++;
            if (TransactionCount == 1)
            {
                GetConnection().StartTransaction();
            }
        }

        public void Commit()
        {
            TransactionCount--;
            if (TransactionCount == 0)
            {
                GetConnection().Commit();
            }
        }

        public void Rollback()
        {
            TransactionCount = 0;
            GetConnection().Rollback();
        }
    }
}
