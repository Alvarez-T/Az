using Dapper;
using System;
using System.Data;

namespace Az.DataSource;

public class Database : IDatabase
{
    private readonly IDbConnectionHandler _connection;

    public Database(IDbConnectionHandler dbConnectionHandler)
    {
        _connection = dbConnectionHandler;
    }

    public IDbConnectionHandler OpenConnection()
    {
        _connection.Open();
        return _connection;
    }

    public IDbConnectionHandler StartOperation()
    {
        _connection.Open();
        _connection.BeginTransaction();
        return _connection;
    }

    public void CloseConnection()
    {
        _connection.Close();
    }

    public void TestConnection()
    {
        _connection.TestConnection();
    }

    public TResult? QueryOperation<TResult>(Func<IDbConnection, TResult> query)
    {
        TResult? queryResult = default;
        ScopedConnection(() => queryResult = query(_connection));
        return queryResult;
    }

    public void ExecuteSql(string sql, object? param = null)
    {
        ScopedConnection(() =>
        {
            _connection.Execute(sql, param);
        });
    }

    private void ScopedConnection(Action method)
    {
        bool localScopedConnection = false;
        if (_connection.State != ConnectionState.Open)
        {
            OpenConnection();
            localScopedConnection = true;
        }

        method.Invoke();

        if (localScopedConnection)
        {
            _connection.Close();
        }
    }
}
