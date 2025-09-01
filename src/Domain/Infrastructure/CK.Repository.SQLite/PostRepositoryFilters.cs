using System;
using System.Collections.Generic;

using CK.Entities;

using Microsoft.Data.Sqlite;

namespace CK.Repository.SQLite
{
    internal static class PostRepositoryFilters
    {
        #region Public Methods

        public static (string, IEnumerable<SqliteParameter>) Author(uint id)
        {
            return ($" {nameof(Post.Author)} = @{nameof(Post.Author)}", new[] { new SqliteParameter($"@{nameof(Post.Author)}", id) });
        }

        public static (string, IEnumerable<SqliteParameter>) BetweenPublished((DateTime Start, DateTime End) range)
        {
            return ($" {nameof(Post.Published)} BETWEEN @{nameof(range.Start)} AND @{nameof(range.End)}", new[]
            {
                new SqliteParameter($"@{nameof(range.Start)}", range.Start.Ticks),
                new SqliteParameter($"@{nameof(range.End)}", range.End.Ticks),
            });
        }

        public static (string, IEnumerable<SqliteParameter>) Description(string description)
        {
            return ($" {nameof(Post.Description)} LIKE @{nameof(Post.Description)}", new[] { new SqliteParameter($"@{nameof(Post.Description)}", description) });
        }

        public static (string, IEnumerable<SqliteParameter>) Id(uint id)
        {
            return ($" {nameof(Post.Id)} = @{nameof(Post.Id)}", new[] { new SqliteParameter($"@{nameof(Post.Id)}", id) });
        }

        public static (string, IEnumerable<SqliteParameter>) IsActive(bool isActive)
        {
            return ($" {nameof(Post.IsActive)} = @{nameof(Post.IsActive)}", new[] { new SqliteParameter($"@{nameof(Post.IsActive)}", isActive) });
        }

        public static (string, IEnumerable<SqliteParameter>) Language(uint id)
        {
            return ($" {nameof(Post.Language)} = @{nameof(Post.Language)}", new[] { new SqliteParameter($"@{nameof(Post.Language)}", id) });
        }

        public static (string, IEnumerable<SqliteParameter>) Snippet(string snippet)
        {
            return ($" {nameof(Post.Snippet)} LIKE @{nameof(Post.Snippet)}", new[] { new SqliteParameter($"@{nameof(Post.Snippet)}", snippet) });
        }

        public static (string, IEnumerable<SqliteParameter>) Title(string title)
        {
            return ($" {nameof(Post.Title)} LIKE @{nameof(Post.Title)}", new[] { new SqliteParameter($"@{nameof(Post.Title)}", title) });
        }

        #endregion Public Methods
    }
}