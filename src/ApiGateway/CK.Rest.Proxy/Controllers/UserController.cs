using CK.Repository;
using CK.Rest.Proxy.Filter;
using CK.Rest.Users.Shared;
using CK.Rest.Users.Shared.Forms;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Proxy.Controllers
{
    /// <summary>
    /// UserController Proxy
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Forward]
    public class UserController : ControllerBase, IUserController
    {
        #region Public Methods

        /// <summary>
        /// List the current users
        /// </summary>
        /// <param name="page">Requested page</param>
        /// <param name="size">Size of items per page (1-10)</param>
        /// <param name="status">The status of the entity</param>
        /// <param name="name">Filter by name</param>
        /// <param name="surname">Filter by surname</param>
        /// <remarks>
        /// Can be execute by anonymous users
        /// <p/>
        /// Only administrator can see the emails, roles and status
        /// </remarks>
        /// <returns>The list of entities</returns>
        /// <response code="200">The list has items</response>
        /// <response code="204">The list has no items</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpGet("{page}/{size}/{status}")]
        public IActionResult Get(ushort page = 1, ushort size = 10, Status status = Status.All, string? name = null, string? surname = null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Return the user
        /// </summary>
        /// <remarks>
        /// Can be execute by anonymous users
        /// <p/>
        /// Only administrator can see the emails, roles and status
        /// </remarks>
        /// <param name="id">The entity's id</param>
        /// <returns>The operation result</returns>
        /// <response code="200">The entity was found</response>
        /// <response code="404">The entity was not found</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpGet("{id}")]
        public IActionResult Get(uint id)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// Needs <b>Authorization</b> header:
        /// <p/>
        /// <i>Authorization : bearer token</i>
        /// <p/>
        /// Can be execute by administrators
        /// </remarks>
        /// <param name="user">The entity's post form</param>
        /// <returns>The operation result</returns>
        /// <response code="201">The user was created</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpPost]
        public IActionResult Post([FromBody] UserFormPost user)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <remarks>
        /// Needs <b>Authorization</b> header:
        /// <p/>
        /// <i>Authorization : bearer token</i>
        /// <p/>
        /// Can be execute only by the same user on self or by an administrator in any user
        /// <p/>
        /// Only administrators can update roles and/or disabled a user
        /// </remarks>
        /// <param name="id">The entity's id</param>
        /// <param name="user">The entity's put form</param>
        /// <returns>The operation result</returns>
        /// <response code="200">The entity was updated</response>
        /// <response code="404">The entity was not found</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpPut("{id}")]
        public IActionResult Put(uint id, [FromBody] UserFormPut user)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        #endregion Public Methods
    }
}