using System.Collections.Immutable;

using CK.Entities;

namespace CK.Repository
{
    public abstract class EntityRepository<T, TKey>
        where T : Entity<TKey>
        where TKey : struct
    {
        #region Public Constructors

        public EntityRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion Public Constructors

        #region Protected Properties

        protected string ConnectionString { get; }

        #endregion Protected Properties

        #region Public Methods

        public abstract Result<T> AddOrUpdate(T entity);

        public abstract bool Exist();

        public abstract Result<T> GetById(TKey id);

        public abstract Result<IImmutableList<T>> ListEntities(
            IImmutableList<Filter<T>> filters = null,
            ushort take = 1,
            ushort skip = 0,
            Status status = Status.All,
            bool desc = false);

        public abstract Result<T> Delete(TKey id);

        #endregion Public Methods
    }
}