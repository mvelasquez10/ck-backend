using CK.Repository;
using CK.Rest.Languages.Shared;
using CK.Rest.Languages.Shared.Forms;
using CK.Rest.Proxy.Filter;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Proxy.Controllers
{
    /// <summary>
    /// LanguageController Proxy
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Forward]
    public class LanguageController : ControllerBase, ILanguageController
    {
        #region Public Methods

        /// <summary>
        /// List the current languages
        /// </summary>
        /// <param name="page">Requested page</param>
        /// <param name="size">Size of items per page (1-10)</param>
        /// <param name="status">The status of the entity</param>
        /// <param name="name">Filter by name</param>
        /// <remarks>
        /// Can be execute by anonymous users
        /// </remarks>
        /// <returns>The list of entities</returns>
        /// <response code="200">The list has items</response>
        /// <response code="204">The list has no items</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpGet("{page}/{size}/{status}")]
        public IActionResult Get(ushort page = 1, ushort size = 10, Status status = Status.All, string? name = null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Return the language
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
        /// Create a new language
        /// </summary>
        /// <remarks>
        /// Needs <b>Authorization</b> header:
        /// <p/>
        /// <i>Authorization : bearer token</i>
        /// <p/>
        /// Can be execute by administrators
        /// </remarks>
        /// <param name="language">The entity's post form</param>
        /// <returns>The operation result</returns>
        /// <response code="201">The user was created</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpPost]
        public IActionResult Post([FromBody] LanguageFormPost language)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Updates the language
        /// </summary>
        /// <remarks>
        /// Needs <b>Authorization</b> header:
        /// <p/>
        /// <i>Authorization : bearer token</i>
        /// <p/>
        /// Can be execute by administrators
        /// </remarks>
        /// <param name="id">The entity's id</param>
        /// <param name="language">The entity's put form</param>
        /// <returns>The operation result</returns>
        /// <response code="200">The entity was updated</response>
        /// <response code="404">The entity was not found</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="422">There was a problem with the repository</response>
        [HttpPut("{id}")]
        public IActionResult Put(uint id, [FromBody] LanguageFormPut language)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        #endregion Public Methods
    }
}