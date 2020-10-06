using System.Text.RegularExpressions;

namespace CK.Common
{
    public static class Extensions
    {
        #region Public Methods

        public static string RemoveSpecialCharacters(this string text)
        {
            return Regex.Replace(text, @"[^0-9a-zA-Z]+", "");
        }

        #endregion Public Methods
    }
}