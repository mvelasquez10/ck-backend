using System.Collections.Immutable;
using System.Linq;

using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Controller;
using CK.Rest.Common.Filters;
using CK.Rest.Common.Shared;
using CK.Rest.Languages.Shared;
using CK.Rest.Languages.Shared.Forms;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Languages.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LanguageController
        : EntityController<Language, uint>, ILanguageController
    {
        #region Public Constructors

        public LanguageController(EntityRepository<Language, uint> repository)
            : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        // GET: api/Language
        [HttpGet]
        [HttpGet("{page}/{size}/{status}")]
        [AllowAnonymous]
        public IActionResult Get(ushort page = 1, ushort size = 10, Status status = Status.All, string? name = null)
        {
            var filters = ImmutableList.Create<Filter<Language>>();
            if (name != null)
                filters = filters.Add(new Filter<Language>(nameof(name).ToCapital(), name + "%"));

            return GetEntities(filters.Any() ? filters : null, page, size, status);
        }

        // GET: api/Language/5
        [HttpGet("{id}", Name = "Get")]
        [AllowAnonymous]
        public IActionResult Get(uint id)
        {
            return GetEntity(id);
        }

        // POST: api/Language
        [HttpPost]
        public IActionResult Post([FromBody] LanguageFormPost form)
        {
            return Post<LanguageFormPost>(form);
        }

        // PUT: api/Language/5
        [HttpPut("{id}")]
        [AdminOrSelf]
        public IActionResult Put(uint id, [FromBody] LanguageFormPut form)
        {
            return Put<LanguageFormPut>(id, form);
        }

        // DELETE: api/Language/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Delete(uint id)
        {
            return DeleteEntity(id);
        }

        #endregion Public Methods
    }
}