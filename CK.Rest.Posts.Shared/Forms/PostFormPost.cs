using System.ComponentModel.DataAnnotations;

using CK.Entities;
using CK.Rest.Common.Shared;
using CK.Rest.Common.Shared.Forms;

namespace CK.Rest.Posts.Shared.Forms
{
    public class PostFormPost : IEntityFormPost<Post, uint>
    {
        #region Public Properties

        public uint? Author { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public uint Language { get; set; }

        [Required]
        public string Snippet { get; set; }

        [Required]
        public string Title { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Post ToEntity(uint id, bool isAdmin = false)
        {
            return new Post(id, Author ?? 0, Title.ToUnescapeDataString(), Description.ToUnescapeDataString(), Language, Snippet.ToUnescapeDataString());
        }

        #endregion Public Methods
    }
}