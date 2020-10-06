using CK.Repository;
using CK.Rest.Posts.Shared;
using CK.Rest.Posts.Shared.Forms;
using CK.Rest.Proxy.Filter;

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
    public class PostController : ControllerBase, IPostController
    {
        #region Public Methods

        /// <summary>
        /// List the current posts
        /// </summary>
        /// <param name="page">Requested page</param>
        /// <param name="size">Size of items per page (1-10)</param>
        /// <param name="status">The status of the entity</param>
        /// <param name="author">Filter by author</param>
        /// <param name="language">Filter by language</param>
        /// <param name="title">Filter by title</param>
        /// <param name="desc">Indicates if the entities should decendent showing newer first, is true by default</param>
        /// <remarks>
        /// Needs <b>Authorization</b> header:
        /// <p/>
        /// <i>Authorization : bearer token</i>
        /// <p/>
        /// Can be execute only by anonymous users
        /// </remarks>
        /// <returns>The list of entities</returns>
        /// <response code="200">The list has items</response>
        /// <response code="204">The list has no items</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpGet("{page}/{size}/{status}")]
        public IActionResult Get(
            ushort page = 1,
            ushort size = 10,
            Status status = Status.All,
            uint? author = null,
            uint? language = null,
            string? title = null,
            bool desc = true)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Return the post
        /// </summary>
        /// <remarks>
        /// Can be execute by anonymous users
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
        /// Create a new post
        /// </summary>
        /// <remarks>
        /// Needs <b>Authorization</b> header:
        /// <p/>
        /// <i>Authorization : bearer token</i>
        /// </remarks>
        /// <param name="post">The entity's post form</param>
        /// <returns>The operation result</returns>
        /// <response code="201">The user was created</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpPost]
        public IActionResult Post([FromBody] PostFormPost post)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Updates the post
        /// </summary>
        /// <remarks>
        /// Needs <b>Authorization</b> header:
        /// <p/>
        /// <i>Authorization : bearer token</i>
        /// <p/>
        /// Can be execute only by the same user on self or by an administrator in any user
        /// </remarks>
        /// <param name="id">The entity's id</param>
        /// <param name="post">The entity's put form</param>
        /// <returns>The operation result</returns>
        /// <response code="200">The entity was updated</response>
        /// <response code="404">The entity was not found</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpPut("{id}")]
        public IActionResult Put(uint id, [FromBody] PostFormPut post)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        #endregion Public Methods
    }
}