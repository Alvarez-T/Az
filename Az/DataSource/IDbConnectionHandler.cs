using System;
using System.Data;

namespace Az.DataSource
{
    public interface IDbConnectionHandler : IDbConnection
    {
        string DatabasePath { get; }
        IDbTransaction? Transaction { get; }

        void DisposeConnection();
        void TestConnection();
    }
}