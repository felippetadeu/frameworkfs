using Framework.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DbConnection
{
    public class MySQLConnection : IConnection
    {
        public MySQLConnection(string connectionString)
        {
            ConnectionString = connectionString;
            Db = new MySqlConnection(connectionString);
            Open();
        }

        public string ConnectionString { get; set; }

        public string SqlParameter
        {
            get
            {
                return "@";
            }
        }

        public System.Data.Common.DbConnection Db { get; private set; }

        public DbTransaction Transaction { get; set; }

        public void Dispose()
        {
            Db.Dispose();
        }

        public void Open()
        {
            if (Db != null)
            {
                Db.Open();
            }
        }

        public void Close()
        {
            if (Db != null)
            {
                Db.Close();
            }
        }

        public bool IsOpen()
        {
            if (Db != null)
            {
                return Db.State == ConnectionState.Open;
            }
            return false;
        }

        public void StartTransaction()
        {
            if (Db != null)
            {
                Transaction = Db.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (Db != null && Transaction != null)
            {
                Transaction.Commit();
                Transaction = null;
            }
        }

        public void Rollback()
        {
            if (Db != null && Transaction != null)
            {
                Transaction.Rollback();
                Transaction = null;
            }
        }
    }
}
