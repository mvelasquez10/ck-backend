using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Entities.Tests
{
    [TestClass]
    public class LanguageTest
    {
        #region Public Methods

        [TestMethod]
        public void CanDeepCopy()
        {
            // Arrange
            var language1 = new Language(1, "Name", false);

            // Act
            var language2 = new Language(language1);

            // Assert
            Assert.IsTrue(language1.Equals(language2));
            Assert.IsTrue(language1.GetHashCode() == language2.GetHashCode());
        }

        [TestMethod]
        public void CreateNewSuccess()
        {
            // Arrange
            uint id = 1;
            var name = "Name";

            // Act
            var language = new Language(id, name);

            // Assert
            Assert.IsTrue(id == language.Id);
            Assert.IsTrue(name == language.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeepCopyObjectIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new Language(null);
        }

        [TestMethod]
        public void DeepCopyWithChanges()
        {
            // Arrange
            uint id = 1;
            var name = "Name";
            var language1 = new Language(2, "Name2");

            // Act
            var language2 = new Language(language1, id, name);

            // Assert
            Assert.IsFalse(language1.Equals(language2 as object));
            Assert.IsTrue(id == language2.Id);
            Assert.IsTrue(name == language2.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NameIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new Language(1, null);
        }

        #endregion Public Methods
    }
}