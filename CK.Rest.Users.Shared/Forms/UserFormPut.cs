using System.ComponentModel.DataAnnotations;

using CK.Entities;
using CK.Rest.Common.Shared;
using CK.Rest.Common.Shared.Forms;

namespace CK.Rest.Users.Shared.Forms
{
    public sealed class UserFormPut : IEntityFormPut<User, uint>
    {
        #region Public Properties

        [EmailAddress]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsAdmin { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        public string? Surname { get; set; }

        #endregion Public Properties

        #region Public Methods

        public User ToEntity(User user, bool isAdmin = false)
        {
            return new User(user, null, Email, Password?.ToSha256(), Name.ToUnescapeDataString(), Surname.ToUnescapeDataString(), isAdmin ? IsActive : null, isAdmin ? IsAdmin : null);
        }

        #endregion Public Methods
    }
}