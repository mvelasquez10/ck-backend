using CK.Entities;

namespace CK.Rest.Common.Shared.Forms
{
    public interface IEntityFormPost<T, TKey>
        where T : Entity<TKey>
        where TKey : struct
    {
        #region Public Methods

        public T ToEntity(TKey id, bool isAdmin = false);

        #endregion Public Methods
    }
}