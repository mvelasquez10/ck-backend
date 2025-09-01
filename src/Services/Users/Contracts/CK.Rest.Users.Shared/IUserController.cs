using CK.Repository;
using CK.Rest.Users.Shared.Forms;

using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Users.Shared
{
    public interface IUserController
    {
        #region Public Methods

        IActionResult Get(uint id);

        IActionResult Get(ushort page = 1, ushort size = 10, Status status = Status.All, string name = null, string surname = null);

        IActionResult Post(UserFormPost form);

        IActionResult Put(uint id, UserFormPut form);

        #endregion Public Methods
    }
}