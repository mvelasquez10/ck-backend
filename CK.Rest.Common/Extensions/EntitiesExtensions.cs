using System;

using CK.Entities;

namespace CK.Rest.Common.Extensions
{
    public static class EntitiesExtensions
    {
        #region Public Methods

        public static object Show<T>(this T entity, bool isAdmin = false)
        {
            return entity switch
            {
                User u => u.ShowUser(isAdmin),
                Language l => l.ShowLanguage(),
                Post p => p.ShowPost(),
                _ => throw new NotImplementedException(typeof(T).Name),
            };
        }

        #endregion Public Methods

        #region Internal Methods

        internal static object ShowLanguage(this Language language)
        {
            return new
            {
                language.Id,
                language.Name,
                language.IsActive,
            };
        }

        internal static object ShowPost(this Post post)
        {
            return new
            {
                post.Id,
                post.Author,
                post.Title,
                post.Description,
                post.Snippet,
                post.Language,
                post.Published,
                post.IsActive,
            };
        }

        internal static object ShowUser(this User user, bool isAdmin)
        {
            if (isAdmin)
            {
                return new
                {
                    user.Id,
                    user.Email,
                    user.Name,
                    user.Surname,
                    user.IsActive,
                    user.IsAdmin,
                };
            }

            return new
            {
                user.Id,
                user.Name,
                user.Surname,
            };
        }

        #endregion Internal Methods
    }
}