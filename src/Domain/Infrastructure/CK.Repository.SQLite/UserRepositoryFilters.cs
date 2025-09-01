using System.Collections.Generic;

using CK.Entities;

using Microsoft.Data.Sqlite;

namespace CK.Repository.SQLite
{
    internal static class UserRepositoryFilters
    {
        #region Public Methods

        public static (string, IEnumerable<SqliteParameter>) Email(string email)
        {
            return ($" {nameof(User.Email)} LIKE @{nameof(User.Email)}", new[] { new SqliteParameter($"@{nameof(User.Email)}", email) });
        }

        public static (string, IEnumerable<SqliteParameter>) Id(uint id)
        {
            return ($" {nameof(User.Id)} = @{nameof(User.Id)}", new[] { new SqliteParameter($"@{nameof(User.Id)}", id) });
        }

        public static (string, IEnumerable<SqliteParameter>) IsActive(bool isActive)
        {
            return ($" {nameof(User.IsActive)} = @{nameof(User.IsActive)}", new[] { new SqliteParameter($"@{nameof(User.IsActive)}", isActive) });
        }

        public static (string, IEnumerable<SqliteParameter>) IsAdmin(bool isAdmin)
        {
            return ($" {nameof(User.IsAdmin)} = @{nameof(User.IsAdmin)}", new[] { new SqliteParameter($"@{nameof(User.IsAdmin)}", isAdmin) });
        }

        public static (string, IEnumerable<SqliteParameter>) Name(string name)
        {
            return ($" {nameof(User.Name)} LIKE @{nameof(User.Name)}", new[] { new SqliteParameter($"@{nameof(User.Name)}", name) });
        }

        public static (string, IEnumerable<SqliteParameter>) NameOrSurname((string name, string surname) fullname)
        {
            return ($" ({nameof(User.Name)} LIKE @{nameof(User.Name)} OR {nameof(User.Surname)} LIKE @{nameof(User.Surname)}) ",
                new[]
                {
                    new SqliteParameter($"@{nameof(User.Name)}", fullname.name),
                    new SqliteParameter($"@{nameof(User.Surname)}", fullname.surname),
                });
        }

        public static (string, IEnumerable<SqliteParameter>) Surname(string name)
        {
            return ($" {nameof(User.Surname)} LIKE @{nameof(User.Surname)}", new[] { new SqliteParameter($"@{nameof(User.Surname)}", name) });
        }

        #endregion Public Methods
    }
}