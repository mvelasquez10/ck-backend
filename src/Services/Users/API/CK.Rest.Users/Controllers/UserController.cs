using System.Collections.Immutable;
using System.Linq;

using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Controller;
using CK.Rest.Common.Extensions;
using CK.Rest.Common.Filters;
using CK.Rest.Common.Shared;
using CK.Rest.Users.Shared;
using CK.Rest.Users.Shared.Forms;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : EntityController<User, uint>, IUserController
    {
        #region Public Constructors

        public UserController(EntityRepository<User, uint> repository)
            : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        // GET: api/User
        [HttpGet]
        [HttpGet("{page}/{size}/{status}")]
        [AllowAnonymous]
        public IActionResult Get(ushort page = 1, ushort size = 10, Status status = Status.All, string? name = null, string? surname = null)
        {
            var filters = ImmutableList.Create<Filter<User>>();
            if (name != null && surname != null)
            {
                filters = filters.Add(new Filter<User>("NameOrSurname", ($"%{name}%", $"%{surname}%")));
            }
            else
            {
                if (name != null)
                    filters = filters.Add(new Filter<User>(nameof(name).ToCapital(), name + "%"));

                if (surname != null)
                    filters = filters.Add(new Filter<User>(nameof(surname).ToCapital(), surname + "%"));
            }

            return GetEntities(filters.Any() ? filters : null, page, size, status);
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        [AllowAnonymous]
        public IActionResult Get(uint id)
        {
            return GetEntity(id);
        }

        // POST: api/User
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Post([FromBody] UserFormPost form)
        {
            return Post<UserFormPost>(form);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [AdminOrSelf]
        public IActionResult Put(uint id, [FromBody] UserFormPut form)
        {
            if (this.IsAdmin() && form.IsAdmin == false)
            {
                var result = Repository.ListEntities(ImmutableList.Create(new[] { new Filter<User>("IsAdmin", true) }), 2);
                if (!result.IsValid)
                    return UnprocessableEntity(result.Exception.Message);

                if (result.Value.Count == 1 && result.Value[0].Id == id)
                {
                    return UnprocessableEntity("Cannot demote the last admin to user");
                }
            }

            return Put<UserFormPut>(id, form);
        }

        #endregion Public Methods
    }
}