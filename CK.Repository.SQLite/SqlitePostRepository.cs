using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;

using CK.Entities;

using Microsoft.Data.Sqlite;

namespace CK.Repository.SQLite
{
    public sealed class SqlitePostRepository : BaseRepository<Post, uint>
    {
        #region Public Constructors

        public SqlitePostRepository(string connectionString)
            : base(connectionString)
        {
        }

        #endregion Public Constructors

        #region Private Enums

        private enum Ordinal
        {
            Id = 0,

            Author,

            Title,

            Description,

            Language,

            Snippet,

            Published,

            IsActive,
        }

        #endregion Private Enums

        #region Protected Properties

        protected override string GetColumns =>
            $"  {nameof(Post.Id)}," +
            $"  {nameof(Post.Author)}," +
            $"  {nameof(Post.Title)}," +
            $"  {nameof(Post.Description)}," +
            $"  {nameof(Post.Language)}," +
            $"  {nameof(Post.Snippet)}," +
            $"  {nameof(Post.Published)}," +
            $"  {nameof(Post.IsActive)} ";

        protected override Type GetFilterResolver => typeof(PostRepositoryFilters);

        protected override string GetTableName => nameof(Post);

        #endregion Protected Properties

        #region Protected Methods

        protected override IImmutableList<Post> BuildEntities(IDataReader reader)
        {
            var entities = ImmutableList.CreateBuilder<Post>();
            while (reader.Read())
            {
                entities.Add(new Post(
                    reader.GetInt64((int)Ordinal.Id).ToUint(),
                    reader.GetInt64((int)Ordinal.Author).ToUint(),
                    reader.GetString((int)Ordinal.Title),
                    reader.GetString((int)Ordinal.Description),
                    reader.GetInt64((int)Ordinal.Language).ToUint(),
                    reader.GetString((int)Ordinal.Snippet),
                    new DateTime(reader.GetInt64((int)Ordinal.Published)),
                    reader.GetBoolean((int)Ordinal.IsActive)));
            }

            return entities.ToImmutable();
        }

        protected override long CreateEntity(Post entity)
        {
            return ExecuteScalar<long>(
                $"INSERT INTO {GetTableName}(" +
                $"  {nameof(Post.Author)}," +
                $"  {nameof(Post.Title)}," +
                $"  {nameof(Post.Description)}," +
                $"  {nameof(Post.Language)}," +
                $"  {nameof(Post.Snippet)}," +
                $"  {nameof(Post.Published)}," +
                $"  {nameof(Post.IsActive)})" +
                $"VALUES(" +
                $"  @{nameof(Post.Author)}," +
                $"  @{nameof(Post.Title)}," +
                $"  @{nameof(Post.Description)}," +
                $"  @{nameof(Post.Language)}," +
                $"  @{nameof(Post.Snippet)}," +
                $"  @{nameof(Post.Published)}," +
                $"  @{nameof(Post.IsActive)});" +
                $"SELECT last_insert_rowid()",
                new List<SqliteParameter>
                {
                    new SqliteParameter($"@{nameof(Post.Author)}", entity.Author),
                    new SqliteParameter($"@{nameof(Post.Title)}", entity.Title),
                    new SqliteParameter($"@{nameof(Post.Description)}", entity.Description),
                    new SqliteParameter($"@{nameof(Post.Language)}", entity.Language),
                    new SqliteParameter($"@{nameof(Post.Snippet)}", entity.Snippet),
                    new SqliteParameter($"@{nameof(Post.Published)}", entity.Published.Ticks),
                    new SqliteParameter($"@{nameof(Post.IsActive)}", entity.IsActive),
                });
        }

        protected override string CreateTable()
        {
            return
                $"CREATE TABLE {GetTableName} (" +
                $"  {nameof(Post.Id)} INTEGER PRIMARY KEY," +
                $"  {nameof(Post.Author)} INTEGER NOT NULL," +
                $"  {nameof(Post.Title)} TEXT NOT NULL," +
                $"  {nameof(Post.Description)} TEXT NOT NULL," +
                $"  {nameof(Post.Language)} INTEGER NOT NULL," +
                $"  {nameof(Post.Snippet)} TEXT NOT NULL," +
                $"  {nameof(Post.Published)} INTEGER NOT NULL," +
                $"  {nameof(User.IsActive)} INTEGER DEFAULT 1)";
        }

        protected override IImmutableList<Post> GetEntities((string Query, IEnumerable<SqliteParameter> Parameters) listQuery)
        {
            return ExecuteReader(listQuery.Query, BuildEntities, listQuery.Parameters);
        }

        protected override void UpdateEntity(Post entity)
        {
            ExecuteNonQuery(
                $"UPDATE {GetTableName} SET " +
                $"  {nameof(Post.Author)} = @{nameof(Post.Author)}, " +
                $"  {nameof(Post.Title)} = @{nameof(Post.Title)}, " +
                $"  {nameof(Post.Description)} = @{nameof(Post.Description)}, " +
                $"  {nameof(Post.Language)} = @{nameof(Post.Language)}," +
                $"  {nameof(Post.Snippet)} = @{nameof(Post.Snippet)}," +
                $"  {nameof(Post.Published)} = @{nameof(Post.Published)}," +
                $"  {nameof(Post.IsActive)} = @{nameof(Post.IsActive)} " +
                $"WHERE " +
                $"  {nameof(entity.Id)} = @{nameof(entity.Id)}",
                new List<SqliteParameter>
                {
                    new SqliteParameter($"@{nameof(Post.Author)}", entity.Author),
                    new SqliteParameter($"@{nameof(Post.Title)}", entity.Title),
                    new SqliteParameter($"@{nameof(Post.Description)}", entity.Description),
                    new SqliteParameter($"@{nameof(Post.Language)}", entity.Language),
                    new SqliteParameter($"@{nameof(Post.Snippet)}", entity.Snippet),
                    new SqliteParameter($"@{nameof(Post.Published)}", entity.Published.Ticks),
                    new SqliteParameter($"@{nameof(Post.IsActive)}", entity.IsActive),
                    new SqliteParameter($"@{nameof(entity.Id)}", entity.Id),
                });
        }

        #endregion Protected Methods
    }
}