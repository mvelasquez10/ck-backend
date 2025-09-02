using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

using CK.Entities;

using Microsoft.Data.Sqlite;

using static CK.Repository.SQLite.ConnectionHelper;

namespace CK.Repository.SQLite
{
    public abstract class BaseRepository<T, TKey> : EntityRepository<T, TKey>
        where T : Entity<TKey>
        where TKey : struct
    {
        #region Private Fields

        private readonly bool _exist;

        #endregion Private Fields

        #region Public Constructors

        public BaseRepository(string connectionString)
            : base(connectionString)
        {
            _exist = TableExist();
        }

        #endregion Public Constructors

        #region Protected Properties

        protected abstract string GetColumns { get; }

        protected abstract Type GetFilterResolver { get; }

        protected abstract string GetTableName { get; }

        #endregion Protected Properties

        #region Public Methods

        public override Result<T> AddOrUpdate(T entity)
        {
            try
            {
                if (entity is null)
                    throw new ArgumentNullException(nameof(entity));

                if (EntityIdExist(entity))
                {
                    uint? id = CreateEntity(entity).ToUint();
                    return new Result<T>(GetEntity(new object[] { entity, id }));
                }
                else
                {
                    UpdateEntity(entity);
                    return new Result<T>(GetEntity(new object[] { entity }));
                }
            }
            catch (Exception ex)
            {
                return new Result<T>(ex);
            }
        }

        public override bool Exist()
        {
            return _exist;
        }

        public override Result<T> GetById(TKey id)
        {
            try
            {
                var result = ExecuteReader(
                    $"SELECT " +
                    $"  {GetColumns} " +
                    $"FROM " +
                    $"  {GetTableName} " +
                    $"WHERE " +
                    $"  {ResolveQuery()} ",
                    r => BuildEntities(r),
                    new List<SqliteParameter>
                    {
                        new SqliteParameter($"@{nameof(Entity<TKey>.Id)}", id),
                    });

                return new Result<T>(result.Any() ? result[0] : throw new ArgumentNullException($"{typeof(T).Name} with id {id} not found"));
            }
            catch (Exception ex)
            {
                return new Result<T>(ex);
            }
        }

        public override Result<IImmutableList<T>> ListEntities(
            IImmutableList<Filter<T>> filters = null,
            ushort take = 1,
            ushort skip = 0,
            Status status = Status.All,
            bool desc = false)
        {
            try
            {
                return new Result<IImmutableList<T>>(GetEntities(BuildListQuery(filters, take, skip, status, desc)));
            }
            catch (Exception ex)
            {
                return new Result<IImmutableList<T>>(ex);
            }
        }

        public override Result<T> Delete(TKey id)
        {
            try
            {
                ExecuteNonQuery(
                    $"DELETE FROM {GetTableName} WHERE {nameof(Entity<TKey>.Id)} = @Id",
                    new List<SqliteParameter>
                    {
                        new SqliteParameter("@Id", id),
                    });

                return new Result<T>((T)null);
            }
            catch (Exception ex)
            {
                return new Result<T>(ex);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract IImmutableList<T> BuildEntities(IDataReader reader);

        protected abstract long CreateEntity(T entity);

        protected abstract string CreateTable();

        protected bool EntityIdExist(Entity<TKey> entity)
        {
            return entity?.Id is null || (ExecuteScalar<long>(
                            $"SELECT " +
                            $"  COUNT(1) " +
                            $"FROM " +
                            $"  {GetTableName} " +
                            $"WHERE " +
                            $"  {ResolveQuery()}",
                            new List<SqliteParameter>
                            {
                                new SqliteParameter($"@{nameof(Entity<TKey>.Id)}", entity.Id),
                            }) == 0);
        }

        protected int ExecuteNonQuery(string query, IEnumerable<SqliteParameter> parameters)
        {
            return Connect(ConnectionString, c => c.ExecuteNonQuery(query, parameters));
        }

        protected IImmutableList<T> ExecuteReader(string query, Func<IDataReader, IImmutableList<T>> map, IEnumerable<SqliteParameter> parameters = null)
        {
            return Connect(ConnectionString, c => c.ExecuteReader(query, r => map(r), parameters));
        }

        protected TStruct ExecuteScalar<TStruct>(string query, IEnumerable<SqliteParameter> parameters)
            where TStruct : struct
        {
            return Connect(ConnectionString, c => c.ExecuteScalar<TStruct>(query, parameters));
        }

        protected abstract IImmutableList<T> GetEntities((string Query, IEnumerable<SqliteParameter> Parameters) listQuery);

        protected abstract void UpdateEntity(T entity);

        #endregion Protected Methods

        #region Private Methods

        private static T GetEntity(object[] arguments)
        {
            return (T)Activator.CreateInstance(
                typeof(T),
                BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding,
                null,
                arguments,
                CultureInfo.CurrentCulture);
        }

        private static string ResolveQuery()
        {
            string query = string.Empty;
            switch (typeof(TKey))
            {
                case Type uintType when uintType == typeof(uint):
                    query = $"{nameof(Entity<TKey>.Id)} = @{nameof(Entity<TKey>.Id)}";
                    break;
            }

            return query;
        }

        private (string Query, IEnumerable<SqliteParameter> Parameters) BuildListQuery(
            IImmutableList<Filter<T>> filters = null,
            ushort take = 1,
            ushort skip = 0,
            Status status = Status.All,
            bool desc = false)
        {
            var sqliteParameters = ImmutableList.CreateBuilder<SqliteParameter>();
            var query = new StringBuilder(
                $"SELECT " +
                $"  {GetColumns} " +
                $"FROM" +
                $"  {GetTableName} ");

            var isFirst = true;
            if (filters != null && filters.Count > 0)
            {
                query.Append("WHERE ");
                foreach (var filter in filters)
                {
                    var (clause, parameters) = filter.ResolveFilter<T, TKey>(GetFilterResolver);

                    query.Append(CultureInfo.InvariantCulture, $" {(isFirst ? string.Empty : "AND ")} {clause} ");

                    foreach (var parameter in parameters)
                    {
                        sqliteParameters.Add(parameter);
                    }

                    if (isFirst)
                        isFirst = false;
                }
            }

            if (status != Status.All)
            {
                if (isFirst)
                {
                    query.Append("WHERE ");
                }
                else
                {
                    query.Append(" AND ");
                }

                query.Append(CultureInfo.InvariantCulture, $" {nameof(Entity<TKey>.IsActive)} = {Convert.ToBoolean((int)status)} ");
            }

            query.Append(
                CultureInfo.InvariantCulture,
                $"ORDER BY " +
                $"  {nameof(Entity<TKey>.Id)} {(desc ? "DESC" : "ASC")} ");

            if (take > 0)
            {
                query.Append(CultureInfo.InvariantCulture, $"LIMIT {take}");
                if (skip > 0)
                    query.Append(CultureInfo.InvariantCulture, $" OFFSET {skip}");
            }

            return (query.ToString(), sqliteParameters);
        }

        private bool TableExist()
        {
            if (Connect(ConnectionString, c => c.ExecuteScalar<long>(
                $"SELECT " +
                $"  count(1) " +
                $"FROM " +
                $"  sqlite_master " +
                $"WHERE " +
                $"  type='table' " +
                $"  AND name=@table",
                new List<SqliteParameter>
                {
                    new SqliteParameter("@table", GetTableName),
                })) == 0)
            {
                Connect(ConnectionString, c => c.ExecuteNonQuery(CreateTable()));
                return false;
            }

            return true;
        }

        #endregion Private Methods
    }
}