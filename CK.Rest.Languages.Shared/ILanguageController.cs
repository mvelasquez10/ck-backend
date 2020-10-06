using CK.Repository;
using CK.Rest.Languages.Shared.Forms;

using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Languages.Shared
{
    public interface ILanguageController
    {
        #region Public Methods

        IActionResult Get(ushort page = 1, ushort size = 10, Status status = Status.All, string? name = null);

        IActionResult Get(uint id);

        IActionResult Post(LanguageFormPost form);

        IActionResult Put(uint id, LanguageFormPut form);

        #endregion Public Methods
    }
}