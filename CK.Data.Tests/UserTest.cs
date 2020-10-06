using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Entities.Tests
{
    [TestClass]
    public class UserTest
    {
        #region Public Methods

        [TestMethod]
        public void CanDeepCopy()
        {
            // Arrange
            var user1 = new User(1, "email", new byte[] { 0x1, 0x1 }, "Name", "Surname", true, true);

            // Act
            var user2 = new User(user1);

            // Assert
            Assert.IsTrue(user1.Equals(user2));
            Assert.IsTrue(user1.GetHashCode() == user2.GetHashCode());
        }

        [TestMethod]
        public void CreateNewSuccess()
        {
            // Arrange
            uint id = 1;
            var email = "Email";
            var pass = new byte[] { 0x1, 0x1 };
            var name = "Name";
            var surname = "Surname";
            var isActive = false;
            var isAdmin = false;

            // Act
            var user = new User(id, email, pass, name, surname, isActive, isAdmin);

            // Assert
            Assert.IsTrue(id == user.Id);
            Assert.IsTrue(name == user.Name);
            Assert.IsTrue(surname == user.Surname);
            Assert.IsTrue(isActive == user.IsActive);
            Assert.IsTrue(isAdmin == user.IsAdmin);
        }

        [TestMethod]
        public void CreateNewSuccessWithDefault()
        {
            // Arrange
            uint id = 1;
            var email = "Email";
            var pass = new byte[] { 0x1, 0x1 };
            var name = "Name";

            // Act
            var user = new User(id, email, pass, name);

            // Assert
            Assert.IsTrue(id == user.Id);
            Assert.IsTrue(name == user.Name);
            Assert.IsTrue(user.Surname == string.Empty);
            Assert.IsTrue(user.IsActive == true);
            Assert.IsTrue(user.IsAdmin == false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeepCopyObjectIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new User(null);
        }

        [TestMethod]
        public void DeepCopyWithChanges()
        {
            // Arrange
            uint id = 1;
            var email = "Email";
            var pass = new byte[] { 0x1, 0x1 };
            var name = "Name";
            var surname = "Surname";
            var isActive = false;
            var isAdmin = false;
            var user1 = new User(2, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);

            // Act
            var user2 = new User(user1, id, email, pass, name, surname, isActive, isAdmin);

            // Assert
            Assert.IsFalse(user1.Equals(user2 as object));
            Assert.IsTrue(id == user2.Id);
            Assert.IsTrue(name == user2.Name);
            Assert.IsTrue(surname == user2.Surname);
            Assert.IsTrue(isActive == user2.IsActive);
            Assert.IsTrue(isAdmin == user2.IsAdmin);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmailIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new User(1, null, new byte[] { 0x1, 0x1 }, "Name");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NameIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new User(1, "email", new byte[] { 0x1, 0x1 }, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassIsRequired()
        {
            // Arrange
            // Act
            // Assert
            new User(1, "email", null, "Name");
        }

        #endregion Public Methods
    }
}