using CK.Entities;
using CK.Rest.Common.Shared;
using CK.Rest.Common.Shared.Forms;

namespace CK.Rest.Languages.Shared.Forms
{
    public class LanguageFormPut : IEntityFormPut<Language, uint>
    {
        #region Public Properties

        public bool? IsActive { get; set; }

        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Language ToEntity(Language language, bool isAdmin = false)
        {
            return new Language(language, null, Name.ToUnescapeDataString(), IsActive);
        }

        #endregion Public Methods
    }
}