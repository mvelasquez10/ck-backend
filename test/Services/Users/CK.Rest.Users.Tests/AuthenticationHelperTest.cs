using System.Collections.Generic;

using CK.Rest.Users.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace CK.Rest.Users.Tests
{
    [TestClass]
    public class AuthenticationHelperTest
    {
        #region Private Fields

        private const string EmailAdmin = "admin@admin.com";

        private const string PassAdmin = "admin1";

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void CanAuthenticate()
        {
            // Arrange
            var userRepo = UserControllerTest.GetMockRepo();
            var repo = new AuthenticationHelper(userRepo, GetConfiguration());
            var user = userRepo.GetById(1).Value;

            // Act
            var result = repo.Authenticate(EmailAdmin, PassAdmin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, user.Id);
            Assert.Equal(result.Name, user.Name);
            Assert.Equal(result.Surname, user.Surname);
            Assert.Equal(result.IsAdmin, user.IsAdmin);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
        }

        [TestMethod]
        public void CannotAuthenticateEmptyParameters()
        {
            // Arrange
            var repo = new AuthenticationHelper(UserControllerTest.GetMockRepo(), GetConfiguration());

            // Act
            var result = repo.Authenticate(string.Empty, string.Empty);

            // Assert
            Assert.Null(result);
        }

        [TestMethod]
        public void CannotAuthenticateInvalidPassword()
        {
            // Arrange
            var repo = new AuthenticationHelper(UserControllerTest.GetMockRepo(), GetConfiguration());

            // Act
            var result = repo.Authenticate(EmailAdmin, "Not my password");

            // Assert
            Assert.Null(result);
        }

        [TestMethod]
        public void CannotAuthenticateInvalidRepository()
        {
            // Arrange
            var repo = new AuthenticationHelper(UserControllerTest.GetMockRepo(false), GetConfiguration());

            // Act
            var result = repo.Authenticate(EmailAdmin, PassAdmin);

            // Assert
            Assert.Null(result);
        }

        #endregion Public Methods

        #region Private Methods

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Common:Secret", "This is a secret just for testing purpose" },
                    { "SessionExpireInDays", "7" },
                }).Build();
        }

        #endregion Private Methods
    }
}