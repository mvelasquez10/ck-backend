using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Filters;
using CK.Rest.Languages.Controllers;
using CK.Rest.Languages.Shared.Forms;
using CK.Rest.TestsBase;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Assert = Xunit.Assert;

namespace CK.Rest.Languages.Tests
{
    [TestClass]
    public class LanguageControllerTest
    {
        #region Public Methods

        [TestMethod]
        public void GetEntityBadRepository()
        {
            // Arrange
            var controller = new LanguageController(GetMockRepo(false));
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
            var controller = new LanguageController(GetMockRepo());
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
            var controller = new LanguageController(GetMockRepo());
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
            var controller = new LanguageController(GetMockRepo(false));

            // Act
            var result = controller.Get(1, 1);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void GetEntitysByName()
        {
            // Arrange
            var controller = new LanguageController(GetMockRepo());

            // Act
            var result = controller.Get(name: "tesTKey") as OkObjectResult;
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
            var controller = new LanguageController(GetMockRepo());

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
            var controller = new LanguageController(GetMockRepo(oversized: true));

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
            var controller = new LanguageController(GetMockRepo());

            // Act
            var result = controller.Get(0, 0);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [TestMethod]
        public void PostEntity()
        {
            // Arrange
            var controller = new LanguageController(GetMockRepo());
            var entity = new LanguageFormPost { Name = "test" };
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
            var controller = new LanguageController(GetMockRepo(false));
            var entity = new LanguageFormPost { Name = "test" };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Post(entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PostEntityBadRepositoryAddOrUpdate()
        {
            var controller = new LanguageController(GetMockRepo(true, false));
            var entity = new LanguageFormPost { Name = "test" };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Post(entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PutEntityBadRepositoryGetId()
        {
            var controller = new LanguageController(GetMockRepo(false));
            var entity = new LanguageFormPut { Name = "test" };
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
            var controller = new LanguageController(GetMockRepo(false));
            var entity = new LanguageFormPut { Name = "test" };
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
            var controller = new LanguageController(GetMockRepo());
            var entity = new LanguageFormPut { Name = "test", IsActive = false };
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
            var controller = new LanguageController(GetMockRepo());
            var entity = new LanguageFormPut { Name = "test" };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(0, entity);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion Public Methods

        #region Internal Methods

        internal static EntityRepository<Language, uint> GetMockRepo(bool good = true, bool addOrUpdateGood = true, bool oversized = false)
        {
            var mockRepo = new Mock<EntityRepository<Language, uint>>("dummy connection string");
            var entities = ImmutableList.Create(
                new Language(1, "test1"),
                new Language(2, "tesTKey"),
                new Language(3, "test3"));

            if (oversized)
            {
                var oversizedList = new List<Language>();
                for (uint i = 4; i < 150; i++)
                {
                    oversizedList.Add(new Language(i, $"test{i}"));
                }

                entities = entities.AddRange(oversizedList);
            }

            mockRepo.Setup(x => x.AddOrUpdate(It.IsAny<Language>()))
                .Returns((Language entity) => good && addOrUpdateGood
                ? new Result<Language>(entity)
                : new Result<Language>(new Exception()));

            mockRepo.Setup(x => x.GetById(It.IsAny<uint>()))
                .Returns((uint id) => good
                ? new Result<Language>(entities.FirstOrDefault(entity => entity.Id == id))
                : new Result<Language>(new Exception()));

            mockRepo.Setup(x => x.ListEntities(
                It.IsAny<IImmutableList<Filter<Language>>>(),
                It.IsAny<ushort>(),
                It.IsAny<ushort>(),
                Status.All,
                It.IsAny<bool>()))
                .Returns((IImmutableList<Filter<Language>> filters, ushort take, ushort skip, Status status, bool desc) => good
                ? new Result<IImmutableList<Language>>(entities.Where(x => filters?.ResolveFilters<Language, uint>(x) ?? true).Take(take).ToImmutableList())
                : new Result<IImmutableList<Language>>(new Exception()));

            return mockRepo.Object;
        }

        #endregion Internal Methods
    }
}