using System.ComponentModel.DataAnnotations;

using CK.Entities;
using CK.Rest.Common.Shared;
using CK.Rest.Common.Shared.Forms;

namespace CK.Rest.Languages.Shared.Forms
{
    public class LanguageFormPost : IEntityFormPost<Language, uint>
    {
        #region Public Properties

        [Required]
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Language ToEntity(uint id, bool isAdmin = false)
        {
            return new Language(id, Name.ToUnescapeDataString());
        }

        #endregion Public Methods
    }
}