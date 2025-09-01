using CK.Entities;
using CK.Rest.Common.Shared;
using CK.Rest.Common.Shared.Forms;

namespace CK.Rest.Posts.Shared.Forms
{
    public class PostFormPut : IEntityFormPut<Post, uint>
    {
        #region Public Properties

        public uint? Author { get; set; }

        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public uint? Language { get; set; }

        public string Snippet { get; set; }

        public string Title { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Post ToEntity(Post entity, bool isAdmin = false)
        {
            return new Post(entity, null, Author, Title.ToUnescapeDataString(), Description.ToUnescapeDataString(), Language, Snippet.ToUnescapeDataString(), isActive: IsActive);
        }

        #endregion Public Methods
    }
}