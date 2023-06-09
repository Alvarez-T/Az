﻿using System;
using System.Data;

namespace Az.DataSource
{
    public interface IDatabase
    {

        void CloseConnection();
        void ExecuteSql(string sql, object? param = null);
        IDbConnectionHandler OpenConnection();
        IDbConnectionHandler StartOperation();
        TResult? QueryOperation<TResult>(Func<IDbConnection, TResult> query);
        void TestConnection();
    }

}