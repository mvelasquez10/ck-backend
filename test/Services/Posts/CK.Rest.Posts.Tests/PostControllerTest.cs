using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Filters;
using CK.Rest.Posts.Controllers;
using CK.Rest.Posts.Shared.Forms;
using CK.Rest.TestsBase;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Assert = Xunit.Assert;

namespace CK.Rest.Posts.Tests
{
    [TestClass]
    public class PostControllerTest
    {
        #region Public Methods

        [TestMethod]
        public void GetEntityBadRepository()
        {
            // Arrange
            var controller = new PostController(GetMockRepo(false));
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
            var controller = new PostController(GetMockRepo());
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
            var controller = new PostController(GetMockRepo());
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
            var controller = new PostController(GetMockRepo(false));

            // Act
            var result = controller.Get(1, 1);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void GetEntitysByAuthor()
        {
            // Arrange
            var controller = new PostController(GetMockRepo());

            // Act
            var result = controller.Get(author: 1) as OkObjectResult;
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
        public void GetEntitysByLanguage()
        {
            // Arrange
            var controller = new PostController(GetMockRepo());

            // Act
            var result = controller.Get(language: 1) as OkObjectResult;
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
        public void GetEntitysByTitle()
        {
            // Arrange
            var controller = new PostController(GetMockRepo());

            // Act
            var result = controller.Get(title: "test1") as OkObjectResult;
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
            var controller = new PostController(GetMockRepo());

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
            var controller = new PostController(GetMockRepo(oversized: true));

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
            var controller = new PostController(GetMockRepo());

            // Act
            var result = controller.Get(0, 0);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [TestMethod]
        public void PostEntity()
        {
            // Arrange
            uint author = 2;
            var controller = new PostController(GetMockRepo());
            var entity = new PostFormPost { Author = 1, Title = "test", Description = "testDescipriton", Language = 1, Snippet = "testSnippet" };
            controller.SetClaimsPrincipal(author, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Post(entity) as CreatedAtActionResult;
            var authorResult = result.Value.GetType().GetProperty("Author").GetValue(result.Value, null);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(author, authorResult);
        }

        [TestMethod]
        public void PostEntityBadRepository()
        {
            // Arrange
            var controller = new PostController(GetMockRepo(false));
            var entity = new PostFormPost { Author = 1, Title = "test", Description = "testDescipriton", Language = 1, Snippet = "testSnippet" };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Post(entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PostEntityBadRepositoryAddOrUpdate()
        {
            var controller = new PostController(GetMockRepo(true, false));
            var entity = new PostFormPut { Title = "test" };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(1, entity);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [TestMethod]
        public void PutEntityAsUserCannotChangeAuthor()
        {
            // Arrange
            uint id = 2;
            var controller = new PostController(GetMockRepo());
            var entity = new PostFormPut { Author = 1, Title = "test", Description = "testDescipriton", Language = 1, Snippet = "testSnippet", IsActive = false };
            controller.SetClaimsPrincipal(id, "admin@admin.com", Role.User);

            // Act
            _ = controller.Put(id, entity);
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);
            var result = controller.Get(id) as OkObjectResult;

            var author = (uint)result.Value.GetType().GetProperty("Author")?.GetValue(result.Value);

            // Assert
            Assert.Equal(author, id);
        }

        [TestMethod]
        public void PutEntityBadRepositoryGetId()
        {
            var controller = new PostController(GetMockRepo(false));
            var entity = new PostFormPut { Title = "test" };
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
            var controller = new PostController(GetMockRepo(false));
            var entity = new PostFormPut { Title = "test" };
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
            var controller = new PostController(GetMockRepo());
            var entity = new PostFormPut { Author = 2, Title = "test", Description = "testDescipriton", Language = 1, Snippet = "testSnippet", IsActive = false };
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
            var controller = new PostController(GetMockRepo());
            var entity = new PostFormPut { Title = "test" };
            controller.SetClaimsPrincipal(1, "admin@admin.com", Role.Admin);

            // Act
            var result = controller.Put(0, entity);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion Public Methods

        #region Internal Methods

        internal static EntityRepository<Post, uint> GetMockRepo(bool good = true, bool addOrUpdateGood = true, bool oversized = false)
        {
            var mockRepo = new Mock<EntityRepository<Post, uint>>("dummy connection string");
            var entities = ImmutableList.Create(
                new Post(1, 1, "test1", "testDescription1", 1, "testSnipper1"),
                new Post(2, 2, "tesTKey", "testDescription2", 2, "testSnipper2"),
                new Post(3, 3, "test3", "testDescription1", 3, "testSnipper3"));

            if (oversized)
            {
                var oversizedList = new List<Post>();
                for (uint i = 4; i < 150; i++)
                {
                    oversizedList.Add(new Post(i, i, $"test{i}", $"testDescription{i}", 3, $"testSnipper{i}"));
                }

                entities = entities.AddRange(oversizedList);
            }

            mockRepo.Setup(x => x.AddOrUpdate(It.IsAny<Post>()))
                .Returns((Post entity) => good && addOrUpdateGood
                ? new Result<Post>(entity)
                : new Result<Post>(new Exception()));

            mockRepo.Setup(x => x.GetById(It.IsAny<uint>()))
                .Returns((uint id) => good
                ? new Result<Post>(entities.FirstOrDefault(entity => entity.Id == id))
                : new Result<Post>(new Exception()));

            mockRepo.Setup(x => x.ListEntities(
                It.IsAny<IImmutableList<Filter<Post>>>(),
                It.IsAny<ushort>(),
                It.IsAny<ushort>(),
                Status.All,
                It.IsAny<bool>()))
                .Returns((IImmutableList<Filter<Post>> filters, ushort take, ushort skip, Status status, bool desc) => good
                ? new Result<IImmutableList<Post>>(entities.Where(x => filters?.ResolveFilters<Post, uint>(x) ?? true).Take(take).ToImmutableList())
                : new Result<IImmutableList<Post>>(new Exception()));

            return mockRepo.Object;
        }

        #endregion Internal Methods
    }
}