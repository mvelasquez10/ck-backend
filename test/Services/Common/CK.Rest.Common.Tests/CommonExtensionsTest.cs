using CK.Rest.Common.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace CK.Rest.Common.Tests
{
    [TestClass]
    public class CommonExtensionsTest
    {
        #region Public Methods

        [TestMethod]
        public void ToCapitalReturnIfEmpty()
        {
            // Arrange
            var test = string.Empty;

            // Act
            // Assert
            Assert.Equal(test.ToCapital(), test);
        }

        [TestMethod]
        public void ToCapitalReturnIfInvalid()
        {
            // Arrange
            var test = "a";

            // Act
            // Assert
            Assert.Equal(test.ToCapital(), test);
        }

        [TestMethod]
        public void ToCapitalSuccess()
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal("test".ToCapital(), "Test");
        }

        #endregion Public Methods
    }
}