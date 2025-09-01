using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;

using CK.Entities;

using Microsoft.Data.Sqlite;

namespace CK.Repository.SQLite
{
    public sealed class SqliteUserRepository : BaseRepository<User, uint>
    {
        #region Public Constructors

        public SqliteUserRepository(string connectionString)
            : base(connectionString)
        {
        }

        #endregion Public Constructors

        #region Private Enums

        private enum Ordinal
        {
            Id = 0,

            Email,

            Pass,

            Name,

            Surname,

            IsActive,

            IsAdmin,
        }

        #endregion Private Enums

        #region Protected Properties

        protected override string GetColumns =>
            $"  {nameof(User.Id)}," +
            $"  {nameof(User.Email)}," +
            $"  {nameof(User.Pass)}," +
            $"  {nameof(User.Name)}," +
            $"  {nameof(User.Surname)}," +
            $"  {nameof(User.IsActive)}," +
            $"  {nameof(User.IsAdmin)} ";

        protected override Type GetFilterResolver => typeof(UserRepositoryFilters);

        protected override string GetTableName => nameof(User);

        #endregion Protected Properties

        #region Protected Methods

        protected override IImmutableList<User> BuildEntities(IDataReader reader)
        {
            var entities = ImmutableList.CreateBuilder<User>();
            while (reader.Read())
            {
                entities.Add(new User(
                    Convert.ToUInt32(reader.GetInt32((int)Ordinal.Id), CultureInfo.InvariantCulture),
                    reader.GetString((int)Ordinal.Email),
                    (byte[])reader.GetValue((int)Ordinal.Pass),
                    reader.GetString((int)Ordinal.Name),
                    reader.GetString((int)Ordinal.Surname),
                    reader.GetBoolean((int)Ordinal.IsActive),
                    reader.GetBoolean((int)Ordinal.IsAdmin)));
            }

            return entities.ToImmutable();
        }

        protected override long CreateEntity(User entity)
        {
            return ExecuteScalar<long>(
                $"INSERT INTO {GetTableName}(" +
                $"  {nameof(User.Email)}," +
                $"  {nameof(User.Pass)}," +
                $"  {nameof(User.Name)}," +
                $"  {nameof(User.Surname)}," +
                $"  {nameof(User.IsActive)}," +
                $"  {nameof(User.IsAdmin)})" +
                $"VALUES(" +
                $"  @{nameof(User.Email)}," +
                $"  @{nameof(User.Pass)}," +
                $"  @{nameof(User.Name)}," +
                $"  @{nameof(User.Surname)}," +
                $"  @{nameof(User.IsActive)}," +
                $"  @{nameof(User.IsAdmin)});" +
                $"SELECT last_insert_rowid()",
                new List<SqliteParameter>
                {
                    new SqliteParameter($"@{nameof(User.Email)}", entity.Email),
                    new SqliteParameter($"@{nameof(User.Pass)}", entity.Pass),
                    new SqliteParameter($"@{nameof(User.Name)}", entity.Name.RemoveSpecialCharacters()),
                    new SqliteParameter($"@{nameof(User.Surname)}", entity.Surname.RemoveSpecialCharacters()),
                    new SqliteParameter($"@{nameof(User.IsActive)}", entity.IsActive),
                    new SqliteParameter($"@{nameof(User.IsAdmin)}", entity.IsAdmin),
                });
        }

        protected override string CreateTable()
        {
            return
                $"CREATE TABLE {GetTableName} (" +
                $"  {nameof(User.Id)} INTEGER PRIMARY KEY," +
                $"  {nameof(User.Email)} TEXT NOT NULL," +
                $"  {nameof(User.Pass)} BLOB NOT NULL," +
                $"  {nameof(User.Name)} TEXT NOT NULL," +
                $"  {nameof(User.Surname)} TEXT," +
                $"  {nameof(User.IsActive)} INTEGER DEFAULT 1," +
                $"  {nameof(User.IsAdmin)} INTEGER DEFAULT 0)";
        }

        protected override IImmutableList<User> GetEntities((string Query, IEnumerable<SqliteParameter> Parameters) listQuery)
        {
            return ExecuteReader(listQuery.Query, BuildEntities, listQuery.Parameters);
        }

        protected override void UpdateEntity(User entity)
        {
            ExecuteNonQuery(
                $"UPDATE {GetTableName} SET " +
                $"  {nameof(User.Email)} = @{nameof(User.Email)}, " +
                $"  {nameof(User.Pass)} = @{nameof(User.Pass)}, " +
                $"  {nameof(User.Name)} = @{nameof(User.Name)}, " +
                $"  {nameof(User.Surname)} = @{nameof(User.Surname)}, " +
                $"  {nameof(User.IsActive)} = @{nameof(User.IsActive)}, " +
                $"  {nameof(User.IsAdmin)} = @{nameof(User.IsAdmin)} " +
                $"WHERE " +
                $"  {nameof(entity.Id)} = @{nameof(entity.Id)}",
                new List<SqliteParameter>
                {
                    new SqliteParameter($"@{nameof(User.Email)}", entity.Email),
                    new SqliteParameter($"@{nameof(User.Pass)}", entity.Pass),
                    new SqliteParameter($"@{nameof(User.Name)}", entity.Name.RemoveSpecialCharacters()),
                    new SqliteParameter($"@{nameof(User.Surname)}", entity.Surname.RemoveSpecialCharacters()),
                    new SqliteParameter($"@{nameof(User.IsActive)}", entity.IsActive),
                    new SqliteParameter($"@{nameof(User.IsAdmin)}", entity.IsAdmin),
                    new SqliteParameter($"@{nameof(entity.Id)}", entity.Id),
                });
        }

        #endregion Protected Methods
    }
}