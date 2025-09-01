using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Entities.Tests
{
    [TestClass]
    public class PostTest
    {
        #region Public Methods

        [TestMethod]
        public void CanDeepCopy()
        {
            // Arrange
            var post1 = new Post(
                1,
                1,
                "Title",
                "Description",
                1,
                "Snippet",
                DateTime.Now,
                true);

            // Act
            var posTKey = new Post(post1);

            // Assert
            Assert.IsTrue(post1.Equals(posTKey));
            Assert.IsTrue(post1.GetHashCode() == posTKey.GetHashCode());
        }

        [TestMethod]
        public void CreateNewSuccess()
        {
            // Arrange
            uint id = 1;
            uint author = 1;
            var title = "Surname";
            var description = "Description";
            uint language = 1;
            var snippet = "Snippet";
            var published = DateTime.Now;
            var isActive = true;

            // Act
            var post = new Post(id, author, title, description, language, snippet, published, isActive);

            // Assert
            Assert.IsTrue(id == post.Id);
            Assert.IsTrue(author == post.Author);
            Assert.IsTrue(title == post.Title);
            Assert.IsTrue(description == post.Description);
            Assert.IsTrue(language == post.Language);
            Assert.IsTrue(snippet == post.Snippet);
            Assert.IsTrue(published == post.Published);
            Assert.IsTrue(isActive == post.IsActive);
            ;
        }

        [TestMethod]
        public void CreateNewSuccessWithDefault()
        {
            // Arrange
            uint id = 1;
            uint author = 1;
            var title = "Surname";
            var description = "Description";
            uint language = 1;
            var snippet = "Snippet";

            // Act
            var post = new Post(id, author, title, description, language, snippet);

            // Assert
            Assert.IsTrue(id == post.Id);
            Assert.IsTrue(author == post.Author);
            Assert.IsTrue(title == post.Title);
            Assert.IsTrue(description == post.Description);
            Assert.IsTrue(language == post.Language);
            Assert.IsTrue(snippet == post.Snippet);
            Assert.IsTrue(post.Published < DateTime.Now);
            Assert.IsTrue(post.IsActive == true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeepCopyObjectIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new Post(null);
        }

        [TestMethod]
        public void DeepCopyWithChanges()
        {
            // Arrange
            uint id = 1;
            uint author = 1;
            var title = "Surname";
            var description = "Description";
            uint language = 1;
            var snippet = "Snippet";
            var published = DateTime.Now;
            var isActive = true;
            var post1 = new Post(
                2,
                2,
                "Title2",
                "Description2",
                2,
                "SnippeTKey",
                DateTime.Now.AddDays(1),
                false);

            // Act
            var posTKey = new Post(post1, id, author, title, description, language, snippet, published, isActive);

            // Assert
            Assert.IsFalse(post1.Equals(posTKey as object));
            Assert.IsTrue(id == posTKey.Id);
            Assert.IsTrue(author == posTKey.Author);
            Assert.IsTrue(title == posTKey.Title);
            Assert.IsTrue(description == posTKey.Description);
            Assert.IsTrue(language == posTKey.Language);
            Assert.IsTrue(snippet == posTKey.Snippet);
            Assert.IsTrue(published == posTKey.Published);
            Assert.IsTrue(isActive == posTKey.IsActive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DescriptionIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new Post(1, 1, "Title", null, 1, "Snippet");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SnippetIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new Post(1, 1, "Title", "Description", 1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TitleIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new Post(1, 1, null, "Description", 1, "Snippet");
        }

        #endregion Public Methods
    }
}