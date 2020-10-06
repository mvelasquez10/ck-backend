using System.ComponentModel.DataAnnotations;

using CK.Entities;
using CK.Rest.Common.Shared;
using CK.Rest.Common.Shared.Forms;

namespace CK.Rest.Users.Shared.Forms
{
    public sealed class UserFormPost : IEntityFormPost<User, uint>
    {
        #region Public Properties

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool? IsAdmin { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public string Surname { get; set; }

        #endregion Public Properties

        #region Public Methods

        public User ToEntity(uint id, bool isAdmin = false)
        {
            return new User(id, Email, Password.ToSha256(), Name.ToUnescapeDataString(), Surname.ToUnescapeDataString(), isAdmin: IsAdmin);
        }

        #endregion Public Methods
    }
}