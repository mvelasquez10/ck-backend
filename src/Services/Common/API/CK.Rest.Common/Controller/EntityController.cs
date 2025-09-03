using System.Collections.Immutable;
using System.Linq;

using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Extensions;
using CK.Rest.Common.Shared.Forms;

using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Common.Controller
{
    public abstract class EntityController<T, TKey> : ControllerBase
        where T : Entity<TKey>
        where TKey : struct
    {
        #region Private Fields

        private const int MaxPageSize = 100;

        #endregion Private Fields

        #region Protected Constructors

        protected EntityController(EntityRepository<T, TKey> repository)
        {
            Repository = repository;
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected EntityRepository<T, TKey> Repository { get; private set; }

        #endregion Protected Properties

        #region Protected Methods

        protected IActionResult GetEntities(
            ImmutableList<Filter<T>> filters = null,
            ushort page = 1,
            ushort size = 20,
            Status status = Status.All,
            bool desc = false)
        {
            if (size > MaxPageSize)
            {
                size = MaxPageSize;
            }

            var result = Repository.ListEntities(filters, size, (ushort)((page - 1) * size), status, desc);

            if (!result.IsValid)
                return UnprocessableEntity(result.Exception.Message);

            var entities = result.Value.Select(x => x.Show(this.IsAdmin()));

            if (!entities.Any())
                return NoContent();

            return Ok(entities);
        }

        protected IActionResult GetEntity(TKey id)
        {
            var result = Repository.GetById(id);

            if (!result.IsValid)
                return UnprocessableEntity(result.Exception.Message);

            if (result.Value is null)
                return NotFound(id);

            return Ok(result.Value.Show(this.IsAdmin()));
        }

        protected IActionResult Post<TPost>([FromBody] TPost form)
            where TPost : IEntityFormPost<T, TKey>
        {
            var result = Repository.AddOrUpdate(form.ToEntity(default, this.IsAdmin()));

            if (!result.IsValid)
                return UnprocessableEntity(result.Exception.Message);

            return CreatedAtAction("Post", result.Value.Show(this.IsAdmin()));
        }

        protected IActionResult Put<TPost>(TKey id, [FromBody] TPost form)
            where TPost : IEntityFormPut<T, TKey>
        {
            var result = Repository.GetById(id);

            if (!result.IsValid)
                return UnprocessableEntity(result.Exception.Message);

            if (result.Value is null)
                return NotFound(id);

            result = Repository.AddOrUpdate(form.ToEntity(result.Value, this.IsAdmin()));

            if (!result.IsValid)
                return UnprocessableEntity(result.Exception.Message);

            return Ok(result.Value.Show(this.IsAdmin()));
        }

        protected IActionResult DeleteEntity(TKey id)
        {
            var result = Repository.Delete(id);

            if (!result.IsValid)
                return UnprocessableEntity(result.Exception.Message);

            if (result.Value is null)
                return NotFound(id);

            return Ok(result.Value.Show(this.IsAdmin()));
        }

        #endregion Protected Methods
    }
}