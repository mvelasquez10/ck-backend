using CK.Rest.Users.Shared.Forms;

namespace CK.Rest.Users.Helpers
{
    public interface IAuthenticationHelper
    {
        #region Public Methods

        UserResultForm Authenticate(string email, string password);

        #endregion Public Methods
    }
}