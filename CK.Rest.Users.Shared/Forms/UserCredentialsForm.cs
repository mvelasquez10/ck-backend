using System.ComponentModel.DataAnnotations;

namespace CK.Rest.Users.Shared.Forms
{
    public sealed class UserCredentialsForm
    {
        #region Public Properties

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        #endregion Public Properties
    }
}