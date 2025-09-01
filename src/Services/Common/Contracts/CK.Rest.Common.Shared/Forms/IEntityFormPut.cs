using CK.Entities;

namespace CK.Rest.Common.Shared.Forms
{
    public interface IEntityFormPut<T, TKey>
        where T : Entity<TKey>
        where TKey : struct
    {
        #region Public Methods

        public T ToEntity(T entity, bool isAdmin = false);

        #endregion Public Methods
    }
}