using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Entities.Tests
{
    [TestClass]
    public class EntityTest
    {
        #region Public Methods

        [TestMethod]
        public void EntityCompareIdFail()
        {
            // Arrange
            var dummy1 = new DummyEntity(1);
            var dummy2 = new DummyEntity(2);

            // Act
            // Assert
            Assert.IsFalse(dummy1.Equals(dummy2 as object));
            Assert.IsTrue(dummy1.GetHashCode() != dummy2.GetHashCode());
            Assert.IsFalse(dummy1.Equals(null));
        }

        [TestMethod]
        public void EntityCompareIdSuccess()
        {
            // Arrange
            var dummy1 = new DummyEntity(1);
            var dummy2 = new DummyEntity(1);

            // Act
            // Assert
            Assert.IsTrue(dummy1.Equals(dummy2));
            Assert.IsTrue(dummy1.GetHashCode() == dummy2.GetHashCode());
        }

        #endregion Public Methods

        #region Internal Classes

        internal class DummyEntity : Entity<uint>
        {
            #region Public Constructors

            public DummyEntity(uint id)
            {
                Id = id;
            }

            #endregion Public Constructors
        }

        #endregion Internal Classes
    }
}