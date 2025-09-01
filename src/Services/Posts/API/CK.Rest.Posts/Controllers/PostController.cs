using System.Collections.Immutable;
using System.Linq;

using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Controller;
using CK.Rest.Common.Extensions;
using CK.Rest.Common.Filters;
using CK.Rest.Common.Shared;
using CK.Rest.Posts.Shared;
using CK.Rest.Posts.Shared.Forms;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Posts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PostController : EntityController<Post, uint>, IPostController
    {
        #region Public Constructors

        public PostController(EntityRepository<Post, uint> repository)
            : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        // GET: api/Post
        [HttpGet]
        [HttpGet("{page}/{size}/{status}")]
        [AllowAnonymous]
        public IActionResult Get(
            ushort page = 1,
            ushort size = 10,
            Status status = Status.All,
            uint? author = null,
            uint? language = null,
            string? title = null,
            bool desc = true)
        {
            var filters = ImmutableList.Create<Filter<Post>>();
            if (author != null)
                filters = filters.Add(new Filter<Post>(nameof(author).ToCapital(), author));

            if (language != null)
                filters = filters.Add(new Filter<Post>(nameof(language).ToCapital(), language));

            if (title != null)
                filters = filters.Add(new Filter<Post>(nameof(title).ToCapital(), $"%{title}%"));

            return GetEntities(filters.Any() ? filters : null, page, size, status, desc);
        }

        // GET: api/Post/5
        [HttpGet("{id}", Name = "Get")]
        [AllowAnonymous]
        public IActionResult Get(uint id)
        {
            return GetEntity(id);
        }

        // POST: api/Post
        [HttpPost]
        public IActionResult Post([FromBody] PostFormPost form)
        {
            form.Author = User.GetId();

            return Post<PostFormPost>(form);
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public IActionResult Put(uint id, [FromBody] PostFormPut form)
        {
            if (!this.IsAdmin())
            {
                form.Author = User.GetId();
            }

            return Put<PostFormPut>(id, form);
        }

        #endregion Public Methods
    }
}