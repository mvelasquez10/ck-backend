using System.Collections.Generic;

using CK.Entities;

using Microsoft.Data.Sqlite;

namespace CK.Repository.SQLite
{
    internal static class LanguageRepositoryFilters
    {
        #region Public Methods

        public static (string, IEnumerable<SqliteParameter>) Id(uint id)
        {
            return ($" {nameof(Language.Id)} = @{nameof(Language.Id)}", new[] { new SqliteParameter($"@{nameof(Language.Id)}", id) });
        }

        public static (string, IEnumerable<SqliteParameter>) IsActive(bool isActive)
        {
            return ($" {nameof(Language.IsActive)} = @{nameof(Language.IsActive)}", new[] { new SqliteParameter($"@{nameof(Language.IsActive)}", isActive) });
        }

        public static (string, IEnumerable<SqliteParameter>) Name(string name)
        {
            return ($" {nameof(Language.Name)} LIKE @{nameof(Language.Name)}", new[] { new SqliteParameter($"@{nameof(Language.Name)}", $"%{name}%") });
        }

        #endregion Public Methods
    }
}