using System;
using System.Data;

namespace Az.DataSource
{
    public interface IDatabase
    {

        void CloseConnection();
        void ExecuteSql(string sql);
        IDbConnectionHandler OpenConnection();
        IDbConnectionHandler StartOperation();
        TResult? Query<TResult>(Func<IDbConnection, TResult> query);
        void TestConnection();
    }

}