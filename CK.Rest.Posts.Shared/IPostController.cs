using CK.Repository;
using CK.Rest.Posts.Shared.Forms;

using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Posts.Shared
{
    public interface IPostController
    {
        #region Public Methods

        IActionResult Get(
            ushort page = 1,
            ushort size = 10,
            Status status = Status.All,
            uint? author = null,
            uint? language = null,
            string? title = null,
            bool desc = true);

        IActionResult Get(uint id);

        IActionResult Post(PostFormPost form);

        IActionResult Put(uint id, PostFormPut form);

        #endregion Public Methods
    }
}