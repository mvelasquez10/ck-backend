using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using CK.Entities;

using Microsoft.Data.Sqlite;

namespace CK.Repository.SQLite
{
    internal static class Extensions
    {
        #region Internal Methods

        internal static TR Method<TR>(this Type t, string method, object obj = null, params object[] parameters) => (TR)t.GetMethod(method).Invoke(obj, parameters);

        internal static string RemoveSpecialCharacters(this string text)
        {
            return Regex.Replace(text, @"[^0-9a-zA-Z]+", string.Empty);
        }

        internal static (string Clause, IEnumerable<SqliteParameter> Parameters) ResolveFilter<T, TKey>(this Filter<T> filter, Type typeResolver)
            where T : Entity<TKey>
            where TKey : struct
        {
            return typeResolver.Method<(string Clause, IEnumerable<SqliteParameter>)>(filter.Property, null, new[] { filter.Value });
        }

        internal static uint ToUint(this long value)
        {
            return Convert.ToUInt32(value, CultureInfo.InvariantCulture);
        }

        #endregion Internal Methods
    }
}