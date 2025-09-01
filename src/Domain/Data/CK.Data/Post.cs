using System;

namespace CK.Entities
{
    public sealed class Post : Entity<uint>, IEquatable<Post>
    {
        #region Public Constructors

        public Post(
           Post post,
           uint? id = null,
           uint? author = null,
           string title = null,
           string description = null,
           uint? language = null,
           string snippet = null,
           DateTime? published = null,
           bool? isActive = null)
        {
            if (post is null)
                throw new ArgumentNullException(nameof(post));

            Id = id ?? post.Id;
            Author = author ?? post.Author;
            Title = title ?? post.Title;
            Description = description ?? post.Description;
            Language = language ?? post.Language;
            Snippet = snippet ?? post.Snippet;
            Published = published ?? post.Published;
            IsActive = isActive ?? post.IsActive;
        }

        public Post(
            uint id,
            uint author,
            string title,
            string description,
            uint language,
            string snippet,
            DateTime? published = null,
            bool? isActive = null)
        {
            Id = id;
            Author = author;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Language = language;
            Snippet = snippet ?? throw new ArgumentNullException(nameof(snippet));
            Published = published ?? DateTime.Now;
            IsActive = isActive ?? true;
        }

        #endregion Public Constructors

        #region Public Properties

        public uint Author { get; }

        public string Description { get; }

        public uint Language { get; }

        public DateTime Published { get; }

        public string Snippet { get; }

        public string Title { get; }

        #endregion Public Properties

        #region Public Methods

        public override bool Equals(object obj)
        {
            return Equals(obj as Post);
        }

        public bool Equals(Post other)
        {
            return other != null &&
                   Author == other.Author &&
                   Description == other.Description &&
                   Id == other.Id &&
                   IsActive == other.IsActive &&
                   Language == other.Language &&
                   Published == other.Published &&
                   Snippet == other.Snippet &&
                   Title == other.Title;
        }

        public override int GetHashCode()
        {
            var hash = default(HashCode);
            hash.Add(Author);
            hash.Add(Description);
            hash.Add(Id);
            hash.Add(IsActive);
            hash.Add(Language);
            hash.Add(Published);
            hash.Add(Snippet);
            hash.Add(Title);
            return hash.ToHashCode();
        }

        #endregion Public Methods
    }
}