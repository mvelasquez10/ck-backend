using CK.Rest.Common.Filters;
using CK.Rest.Common.Shared.Forms;
using CK.Rest.TestsBase;
using CK.Rest.Users.Controllers;
using CK.Rest.Users.Form;
using CK.Rest.Users.Helpers;
using CK.Rest.Users.Shared.Forms;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Assert = Xunit.Assert;

namespace CK.Rest.Users.Tests
{
    [TestClass]
    public class AuthControllerTest
    {
        #region Private Fields

        private const string EmailAdmin = "admin@admin.com";

        private const string GoodToken = "valid token";

        private const string NameAdmin = "System";

        private const string PassAdmin = "admin";

        private const string SurnameAdmin = "Admin";

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void CanAuthenticate()
        {
            // Arrange
            var controller = new AuthController(GetMockRepo());
            var user = new UserCredentialsForm { Email = EmailAdmin, Password = PassAdmin };

            // Act
            var result = controller.Post(user) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UserResultForm>(result.Value);
        }

        [TestMethod]
        public void CannotAuthenticate()
        {
            // Arrange
            var controller = new AuthController(GetMockRepo());
            var user = new UserCredentialsForm { Email = "Bad User", Password = " Bad Pass" };

            // Act
            var result = controller.Post(user) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ErrorResult>(result.Value);
            Assert.False(string.IsNullOrWhiteSpace(((ErrorResult)result.Value).Error));
        }

        [TestMethod]
        public void CheckIsAuthenticate()
        {
            // Arrange
            var controller = new AuthController(GetMockRepo());
            controller.SetClaimsPrincipal(1, EmailAdmin, Role.Admin);

            // Act
            var result = controller.IsAuthenticate();

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [TestMethod]
        public void CheckIsNotAuthenticate()
        {
            // Arrange
            var controller = new AuthController(GetMockRepo());
            controller.SetEmptyContext();

            // Act
            var result = controller.IsAuthenticate();

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        #endregion Public Methods

        #region Private Methods

        private static IAuthenticationHelper GetMockRepo()
        {
            var mockRepo = new Mock<IAuthenticationHelper>();

            mockRepo.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns((string email, string pass) =>
            email == EmailAdmin && pass == PassAdmin ? new UserResultForm
            {
                Id = 1,
                IsAdmin = true,
                Name = NameAdmin,
                Surname = SurnameAdmin,
                Token = GoodToken,
            }
            : null);

            return mockRepo.Object;
        }

        #endregion Private Methods
    }
}