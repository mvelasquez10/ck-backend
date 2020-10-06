using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CK.Rest.Common.Shared
{
    public static class CommonExtensions
    {
        #region Public Methods

        public static string ToCapital(this string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < 2)
                return text;

            var lower = text.ToLowerInvariant();
            return lower[0].ToString().ToUpperInvariant() + lower[1..];
        }

        public static IEnumerable<byte> ToSha256(this string text)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            using var result = SHA256.Create();
            return result.ComputeHash(textBytes);
        }

        public static string ToUnescapeDataString(this string property)
        {
            return string.IsNullOrWhiteSpace(property) ? property : Uri.UnescapeDataString(property);
        }

        #endregion Public Methods
    }
}