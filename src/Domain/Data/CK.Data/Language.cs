using System;

namespace CK.Entities
{
    public sealed class Language : Entity<uint>, IEquatable<Language>
    {
        #region Public Constructors

        public Language(
            uint id,
            string name,
            bool isActive = true)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsActive = isActive;
        }

        public Language(
            Language language,
            uint? id = null,
            string name = null,
            bool? isActive = null)
        {
            if (language is null)
                throw new ArgumentNullException(nameof(language));

            Id = id ?? language.Id;
            Name = name ?? language.Name;
            IsActive = isActive ?? language.IsActive;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public override bool Equals(object obj)
        {
            return Equals(obj as Language);
        }

        public bool Equals(Language other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   IsActive == other.IsActive;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, IsActive);
        }

        #endregion Public Methods
    }
}