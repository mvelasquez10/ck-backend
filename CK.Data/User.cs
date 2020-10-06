using System;
using System.Collections.Generic;
using System.Linq;

namespace CK.Entities
{
    public sealed class User : Entity<uint>, IEquatable<User>
    {
        #region Public Constructors

        public User(
            uint id,
            string email,
            IEnumerable<byte> pass,
            string name,
            string surname = null,
            bool? isActive = null,
            bool? isAdmin = null)
        {
            Id = id;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Pass = pass ?? throw new ArgumentNullException(nameof(pass));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? string.Empty;
            IsActive = isActive ?? true;
            IsAdmin = isAdmin ?? false;
        }

        public User(
            User user,
            uint? id = null,
            string? email = null,
            IEnumerable<byte>? pass = null,
            string name = null,
            string surname = null,
            bool? isActive = null,
            bool? isAdmin = null)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            Id = id ?? user.Id;
            Email = email ?? user.Email;
            Pass = pass ?? user.Pass;
            Name = name ?? user.Name;
            Surname = surname ?? user.Surname;
            IsActive = isActive ?? user.IsActive;
            IsAdmin = isAdmin ?? user.IsAdmin;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Email { get; }

        public bool IsAdmin { get; }

        public string Name { get; }

        public IEnumerable<byte> Pass { get; }

        public string Surname { get; }

        #endregion Public Properties

        #region Public Methods

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User other)
        {
            return other != null &&
                   Id == other.Id &&
                   Email == other.Email &&
                   Pass.SequenceEqual(other.Pass) &&
                   IsActive == other.IsActive &&
                   IsAdmin == other.IsAdmin &&
                   Name == other.Name &&
                   Surname == other.Surname;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Email, Pass, IsActive, IsAdmin, Name, Surname);
        }

        #endregion Public Methods
    }
}