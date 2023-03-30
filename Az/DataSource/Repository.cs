﻿using Dapper;

namespace Az.DataSource;

public abstract class Repository<T> : IRepository
{
    protected IDatabase Database;

    public Repository(IDatabase database)
    {
        this.Database = database;
    }

    protected void ExecuteSql(string sql, object? param = null)
    {
        Database.ExecuteSql(sql, param);
    }

    protected void ExecuteSqlAsync(string sql, params object[] args) 
    {
        throw new NotImplementedException();
    }


    protected T? QuerySingleOrDefault(string sql)
    {
        return Database.Query(connection => connection.QuerySingleOrDefault<T>(sql));
    }

    protected T? QueryFirstOrDefault(string sql)
    {
        return Database.Query(connection => connection.QueryFirstOrDefault<T>(sql));
    }

    protected T QueryFirst(string sql)
    {
        return Database.Query(connection => connection.QueryFirst<T>(sql))!;
    }

    protected IEnumerable<T> Query(string sql)
    {
        return Database.Query(connection => connection.Query<T>(sql))!;
    }

    protected IEnumerable<TResult> Query<TResult>(string sql)
    {
        return Database.Query(connection => connection.Query<TResult>(sql))!;
    }

    protected IEnumerable<T> QueryWith<TChild, TChild2, TChild3>(string sql, Func<T, TChild, TChild2, TChild3, T> map, string splitOn = "")
    {
        try
        {
            using (var connection = Database.OpenConnection())

                return connection.Query(sql, map, null, null, splitOn: splitOn);
        }
        catch (Exception ex)
        {
            throw;
            //log
        }

    }

}

