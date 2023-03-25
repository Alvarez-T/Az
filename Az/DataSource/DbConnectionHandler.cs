using Az.Extensions;
using System;
using System.Data;
using System.Threading;

namespace Az.DataSource;

public class DbConnectionHandler : IDbConnectionHandler
{
    private readonly IDbConnection _connection;
    private int tentativasDeReconexao = 3;

    public IDbTransaction? Transaction { get; private set; }

    public DbConnectionHandler(IDbConnection dbConnection, string databasePath)
    {
        _connection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

        if (string.IsNullOrWhiteSpace(databasePath))
            throw new ArgumentNullException(nameof(databasePath));

        DatabasePath = databasePath;
    }


    public string DatabasePath { get; }

    public string ConnectionString
    {
        get => _connection.ConnectionString;
        set => _connection.ConnectionString = value;
    }

    public int ConnectionTimeout => _connection.ConnectionTimeout;
    public string Database => _connection.Database;
    public ConnectionState State => _connection.State;

    public void Open()
    {
        do
        {
            try
            {
                _connection.Open();
            }
            catch
            {
                if (ConnectionString.IsEmpty())
                    throw new NotImplementedException();

                Thread.Sleep(500);
                tentativasDeReconexao--;
            }
        } while (tentativasDeReconexao > 0 && _connection.State != ConnectionState.Open);

        if (_connection.State != ConnectionState.Open)
            throw new ClosedConnectionException($"Não foi possível comunicar-se com o Banco de dados {DatabasePath}");
    }

    public void Close()
    {
        if (Transaction != null)
            Transaction.Dispose();

        _connection.Close();
    }

    public IDbTransaction BeginTransaction()
    {
        Transaction = _connection.BeginTransaction();
        return Transaction;
    }

    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
        Transaction = _connection.BeginTransaction(il);
        return Transaction;
    }

    public void ChangeDatabase(string databaseName)
    {
        _connection.ChangeDatabase(databaseName);
    }

    public IDbCommand CreateCommand()
    {
        if (Transaction is null)
            this.BeginTransaction();

        var cmd = _connection.CreateCommand();
        cmd.Transaction = Transaction;
        return cmd;
    }

    public void Dispose()
    {
        Close();
    }

    public void DisposeConnection()
    {
        _connection.Dispose();
    }

    public void TestConnection()
    {
        this.Open();
        this.Close();
    }
}
