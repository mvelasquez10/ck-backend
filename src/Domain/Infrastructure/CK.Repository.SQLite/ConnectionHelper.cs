using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Microsoft.Data.Sqlite;

namespace CK.Repository.SQLite
{
    internal static class ConnectionHelper
    {
        #region Internal Methods

        internal static T Connect<T>(string connectionString, Func<SqliteConnection, T> map)
        {
            return new SqliteConnection(connectionString).Using(
                conn =>
                {
                    conn.Open();
                    return map(conn);
                });
        }

        internal static int ExecuteNonQuery(this SqliteConnection connection, string query, IEnumerable<SqliteParameter> parameters = null)
        {
            var command = GetCommand(connection, query, parameters);
            return command.ExecuteNonQuery();
        }

        internal static TResult ExecuteReader<TResult>(this SqliteConnection connection, string query, Func<IDataReader, TResult> map, IEnumerable<SqliteParameter> parameters = null)
        {
            var command = GetCommand(connection, query, parameters);
            return map(command.ExecuteReader());
        }

        internal static TResult ExecuteScalar<TResult>(this SqliteConnection connection, string query, IEnumerable<SqliteParameter> parameters = null)
            where TResult : struct
        {
            var command = GetCommand(connection, query, parameters);
            return (TResult)command.ExecuteScalar();
        }

        internal static T Using<T, TDisposable>(this TDisposable disposable, Func<TDisposable, T> map)
                                    where TDisposable : IDisposable
        {
            using (disposable)
                return map(disposable);
        }

        #endregion Internal Methods

        #region Private Methods

        private static SqliteCommand GetCommand(SqliteConnection connection, string query, IEnumerable<SqliteParameter> parameters)
        {
            var command = new SqliteCommand(query, connection);
            if (parameters != null && parameters.Any())
            {
                command.Parameters.AddRange(parameters);
                command.Prepare();
            }

            return command;
        }

        #endregion Private Methods
    }
}