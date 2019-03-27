using System;

namespace Framework.Interfaces
{
    public interface IConnection: IDisposable
    {
        string ConnectionString { get; }
        string SqlParameter { get; }
        System.Data.Common.DbConnection Db { get; }
        System.Data.Common.DbTransaction Transaction { get; }

        void Open();
        void Close();
        bool IsOpen();
        void StartTransaction();
        void Commit();
        void Rollback();
    }
}
