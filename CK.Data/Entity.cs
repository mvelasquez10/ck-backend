using System;

namespace CK.Entities
{
    public abstract class Entity<TKey> : IEquatable<Entity<TKey>>
        where TKey : struct
    {
        #region Public Properties

        public TKey Id { get; protected set; }

        public bool IsActive { get; protected set; }

        #endregion Public Properties

        #region Public Methods

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TKey>);
        }

        public bool Equals(Entity<TKey> other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   IsActive == IsActive;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, IsActive);
        }

        #endregion Public Methods
    }
}