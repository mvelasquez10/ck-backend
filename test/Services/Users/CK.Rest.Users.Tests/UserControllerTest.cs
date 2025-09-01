using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Filters;
using CK.Rest.Common.Shared;
using CK.Rest.TestsBase;
using CK.Rest.Users.Controllers;
using CK.Rest.Users.Shared.Forms;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Assert = Xunit.Assert;

namespace CK.Rest.Users.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        #region Public Methods

        [TestMethod]
        public void GetEntityAsUserShowNoEmail()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());
            controller.SetClaimsPrincipal(1, "user@user.com", Role.User);
            uint id = 1;

            // Act
            var result = controller.Get(id) as OkObjectResult;
            var email = result.Value.GetType().GetProperty("Email");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Null(email);
        }

        [TestMethod]
        public void GetEntityBadRepository()
        {
            // Arrange
            var controller = new UserController(GetMockRepo(false));
            uint id = 1;

            // Act
            var result = controller.Get(id);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void GetEntityExist()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());
            uint id = 1;

            // Act
            var result = controller.Get(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [TestMethod]
        public void GetEntityNoExist()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());
            uint id = 0;

            // Act
            var result = controller.Get(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [TestMethod]
        public void GetEntitysBadRepository()
        {
            // Arrange
            var controller = new UserController(GetMockRepo(false));

            // Act
            var result = controller.Get(1, 1);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void GetEntitysByName()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());

            // Act
            var result = controller.Get(name: "user2") as OkObjectResult;
            var list = result.Value as IEnumerable;
            var amount = 0;
            foreach (var item in list)
            {
                amount++;
            }

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(amount == 1);
        }

        [TestMethod]
        public void GetEntitysByNameOrSurname()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());

            // Act
            var result = controller.Get(name: "user2", surname: "user2surname") as OkObjectResult;
            var list = result.Value as IEnumerable;
            var amount = 0;
            foreach (var item in list)
            {
                amount++;
            }

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(amount == 1);
        }

        [TestMethod]
        public void GetEntitysBySurname()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());

            // Act
            var result = controller.Get(surname: "user2surname") as OkObjectResult;
            var list = result.Value as IEnumerable;
            var amount = 0;
            foreach (var item in list)
            {
                amount++;
            }

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(amount == 1);
        }

        [TestMethod]
        public void GetEntitysExist()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());

            // Act
            var result = controller.Get(1, 1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [TestMethod]
        public void GetEntitysMaxPage()
        {
            // Arrange
            var maxAmount = 100;
            var controller = new UserController(GetMockRepo(oversized: true));

            // Act
            var result = controller.Get(1, 1000) as OkObjectResult;
            var list = result.Value as IEnumerable;
            var amount = 0;
            foreach (var item in list)
            {
                amount++;
                if (amount > maxAmount)
                    break;
            }

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(amount <= maxAmount);
        }

        [TestMethod]
        public void GetEntitysNoExist()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());

            // Act
            var result = controller.Get(0, 0);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [TestMethod]
        public void PostEntity()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());
            var entity = new UserFormPost { Email = "test", Password = "test", Name = "test", Surname = "test", IsAdmin = true };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Post(entity);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [TestMethod]
        public void PostEntityBadRepository()
        {
            // Arrange
            var controller = new UserController(GetMockRepo(false));
            var entity = new UserFormPost { Email = "test", Password = "test", Name = "test", Surname = "test", IsAdmin = true };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Post(entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PostEntityBadRepositoryAddOrUpdate()
        {
            var controller = new UserController(GetMockRepo(true, false));
            var entity = new UserFormPut { Email = "test", Password = "test", Name = "test", Surname = "test", IsActive = true, IsAdmin = true };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(1, entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PutEntityAsUserCannotChangeIsActive()
        {
            // Arrange
            uint id = 2;
            var controller = new UserController(GetMockRepo());
            var entity = new UserFormPut { Email = "test", Password = "test", Name = "test", Surname = "test", IsActive = false };
            controller.SetClaimsPrincipal(id, "admin@admin.com", Role.User);

            // Act
            _ = controller.Put(id, entity);
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);
            var result = controller.Get(id) as OkObjectResult;

            var isActive = (bool)result.Value.GetType().GetProperty("IsActive")?.GetValue(result.Value);

            // Assert
            Assert.True(isActive);
        }

        [TestMethod]
        public void PutEntityAsUserCannotChangeIsAdmin()
        {
            // Arrange
            uint id = 3;
            var controller = new UserController(GetMockRepo());
            var entity = new UserFormPut { Email = "test", Password = "test", Name = "test", Surname = "test", IsAdmin = true };
            controller.SetClaimsPrincipal(id, "admin@admin.com", Role.User);

            // Act
            _ = controller.Put(id, entity);
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);
            var result = controller.Get(id) as OkObjectResult;

            var isAdmin = (bool)result.Value.GetType().GetProperty("IsAdmin")?.GetValue(result.Value);

            // Assert
            Assert.False(isAdmin);
        }

        [TestMethod]
        public void PutEntityBadRepositoryGetId()
        {
            var controller = new UserController(GetMockRepo(false));
            var entity = new UserFormPut { Email = "test", Password = "test", Name = "test", Surname = "test", IsActive = true, IsAdmin = true };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(1, entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PutEntityBadRespository()
        {
            // Arrange
            var controller = new UserController(GetMockRepo(false));
            var entity = new UserFormPut { Email = "test", Password = "test", Name = "test", Surname = "test", IsActive = true, IsAdmin = true };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(1, entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PutEntityCanDemoteOtherAdmin()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());
            var entity = new UserFormPut { IsAdmin = false };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(2, entity) as OkObjectResult;
            var isAdmin = (bool)result.Value.GetType().GetProperty("IsAdmin")?.GetValue(result.Value);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.False(isAdmin);
        }

        [TestMethod]
        public void PutEntityCannotDemoteLastAdmin()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());
            var entity = new UserFormPut { IsAdmin = false };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            _ = controller.Put(2, entity);
            var result = controller.Put(1, entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PutEntityCannotDemoteLastAdminBadRepo()
        {
            // Arrange
            var controller = new UserController(GetMockRepo(false));
            var entity = new UserFormPut { IsAdmin = false };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(1, entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PutEntityExist()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());
            var entity = new UserFormPut { Email = "test", Password = "test", Name = "test", Surname = "test", IsActive = true, IsAdmin = true };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(1, entity);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [TestMethod]
        public void PutEntityNoExist()
        {
            // Arrange
            var controller = new UserController(GetMockRepo());
            var entity = new UserFormPut { Email = "test", Password = "test" };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(0, entity);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion Public Methods

        #region Internal Methods

        internal static EntityRepository<User, uint> GetMockRepo(bool good = true, bool addOrUpdateGood = true, bool oversized = false)
        {
            var mockRepo = new Mock<EntityRepository<User, uint>>("dummy connection string");
            var entities = new List<User>
            {
                new User(1, "admin@admin.com", "admin1".ToSha256(), "admin1", "admin1surname", isAdmin: true),
                new User(2, "admin@admin.com", "admin2".ToSha256(), "admin2", "admin2surname", isAdmin: true),
                new User(3, "user1@user.com", "user1".ToSha256(), "user1", "user1surname"),
                new User(4, "user2@user.com", "user2".ToSha256(), "user2", "user2surname"),
            };

            if (oversized)
            {
                var oversizedList = new List<User>();
                for (uint i = 5; i < 150; i++)
                {
                    oversizedList.Add(new User(i, $"user{i}@user.com", new byte[] { 0x01 }, $"user{i}", $"user{i}surname"));
                }

                entities.AddRange(oversizedList);
            }

            mockRepo.Setup(x => x.AddOrUpdate(It.IsAny<User>()))
                .Returns((User entity) =>
                {
                    entities.RemoveAll(x => x.Id == entity.Id);
                    entities.Add(entity);
                    return good && addOrUpdateGood
                    ? new Result<User>(entity)
                    : new Result<User>(new Exception());
                });

            mockRepo.Setup(x => x.GetById(It.IsAny<uint>()))
                .Returns((uint id) => good
                ? new Result<User>(entities.FirstOrDefault(entity => entity.Id == id))
                : new Result<User>(new Exception()));

            mockRepo.Setup(x => x.ListEntities(
                It.IsAny<IImmutableList<Filter<User>>>(),
                It.IsAny<ushort>(),
                It.IsAny<ushort>(),
                Status.All,
                It.IsAny<bool>()))
                .Returns((IImmutableList<Filter<User>> filters, ushort take, ushort skip, Status status, bool desc) => good
                ? new Result<IImmutableList<User>>(entities.Where(x => filters?.ResolveFilters<User, uint>(x) ?? true).Take(take).ToImmutableList())
                : new Result<IImmutableList<User>>(new Exception()));

            return mockRepo.Object;
        }

        #endregion Internal Methods
    }
}