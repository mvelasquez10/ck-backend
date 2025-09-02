using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;

using CK.Entities;

using Microsoft.Data.Sqlite;

namespace CK.Repository.SQLite
{
    public sealed class SqliteLanguageRepository : BaseRepository<Language, uint>
    {
        #region Public Constructors

        public SqliteLanguageRepository(string connectionString)
            : base(connectionString)
        {
        }

        #endregion Public Constructors

        #region Private Enums

        private enum Ordinal
        {
            Id = 0,

            Name,

            IsActive,
        }

        #endregion Private Enums

        #region Protected Properties

        protected override string GetColumns =>
            $"  {nameof(Language.Id)}," +
            $"  {nameof(Language.Name)}," +
            $"  {nameof(Language.IsActive)} ";

        protected override Type GetFilterResolver => typeof(LanguageRepositoryFilters);

        protected override string GetTableName => nameof(Language);

        #endregion Protected Properties

        #region Protected Methods

        protected override IImmutableList<Language> BuildEntities(IDataReader reader)
        {
            var entities = ImmutableList.CreateBuilder<Language>();
            while (reader.Read())
            {
                entities.Add(new Language(
                    Convert.ToUInt32(reader.GetInt32((int)Ordinal.Id), CultureInfo.InvariantCulture),
                    reader.GetString((int)Ordinal.Name),
                    reader.GetBoolean((int)Ordinal.IsActive)));
            }

            return entities.ToImmutable();
        }

        protected override long CreateEntity(Language entity)
        {
            return ExecuteScalar<long>(
                $"INSERT INTO {GetTableName}(" +
                $"  {nameof(Language.Name)}," +
                $"  {nameof(Language.IsActive)})" +
                $"VALUES(" +
                $"  @{nameof(Language.Name)}," +
                $"  @{nameof(Language.IsActive)});" +
                $"SELECT last_insert_rowid()",
                new List<SqliteParameter>
                {
                    new SqliteParameter($"@{nameof(Language.Name)}", entity.Name),
                    new SqliteParameter($"@{nameof(Language.IsActive)}", entity.IsActive),
                });
        }

        protected override string CreateTable()
        {
            return
                $"CREATE TABLE {GetTableName} (" +
                $"  {nameof(Language.Id)} INTEGER PRIMARY KEY," +
                $"  {nameof(Language.Name)} TEXT NOT NULL," +
                $"  {nameof(Language.IsActive)} INTEGER DEFAULT 1)";
        }

        protected override IImmutableList<Language> GetEntities((string Query, IEnumerable<SqliteParameter> Parameters) listQuery)
        {
            return ExecuteReader(listQuery.Query, BuildEntities, listQuery.Parameters);
        }

        protected override void UpdateEntity(Language entity)
        {
            ExecuteNonQuery(
                $"UPDATE {GetTableName} SET " +
                $"  {nameof(Language.Name)} = @{nameof(Language.Name)}, " +
                $"  {nameof(Language.IsActive)} = @{nameof(Language.IsActive)} " +
                $"WHERE " +
                $"  {nameof(entity.Id)} = @{nameof(entity.Id)}",
                new List<SqliteParameter>
                {
                    new SqliteParameter($"@{nameof(Language.Name)}", entity.Name),
                    new SqliteParameter($"@{nameof(Language.IsActive)}", entity.IsActive),
                    new SqliteParameter($"@{nameof(entity.Id)}", entity.Id),
                });
        }

        #endregion Protected Methods
    }
}