using System;
using System.Collections.Immutable;
using System.Linq;

using CK.Entities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Repository.SQLite.Tests
{
    [TestClass]
    public class PostRepositoryTest : BaseRepositoryTest
    {
        #region Public Methods

        [TestMethod]
        public void AddOrUpdateEntityIsRequired()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);

            // Act
            var result = repo.AddOrUpdate(null);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Exception is ArgumentNullException);
        }

        [TestMethod]
        public void AddOrUpdateIsSafe()
        {
            // Arrange
            var conn = GetConnectionString();
            var entity = new Post(0, 0, "Title", "Description", 0, "Snippet");
            var repo = new SqlitePostRepository(conn);

            // Act
            _conn.Dispose();
            var newEntity = repo.AddOrUpdate(entity);

            // Assert
            Assert.IsFalse(newEntity.IsValid);
        }

        [TestMethod]
        public void CanCreateEntity()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity = new Post(0, 0, "Title", "Description", 0, "Snippet");

            // Act
            var newEntity = repo.AddOrUpdate(entity);

            // Assert
            Assert.IsTrue(newEntity.IsValid);
            Assert.IsTrue(newEntity.Value.Id == 1);
        }

        [TestMethod]
        public void CanGetEntityById()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity = new Post(0, 0, "Title", "Description", 0, "Snippet");
            var savedEntity = repo.AddOrUpdate(entity);

            // Act
            var result = repo.GetById(savedEntity.Value.Id);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Value.Equals(savedEntity.Value));
        }

        [TestMethod]
        public void CanGetEntityByIdNotFound()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqliteLanguageRepository(conn);

            // Act
            var result = repo.GetById(0);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void CanListEntities()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqlitePostRepository(conn);
            var entity = new Post(0, 0, "Title", "Description", 0, "Snippet");
            var savedEntity = repo.AddOrUpdate(entity);

            // Act
            var listEntities = repo.ListEntities();

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesActive()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 0, "Title1", "Description1", 0, "Snippet1", isActive: true);
            var savedEntity1 = repo.AddOrUpdate(entity1);
            var entity2 = new Post(0, 0, "Title2", "Description2", 0, "SnippeTKey", isActive: false);
            _ = repo.AddOrUpdate(entity2);

            // Act
            var listEntities = repo.ListEntities(status: Status.Active);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Count == 1);
            Assert.IsTrue(savedEntity1.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesActiveAndByTitle()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 0, "Title1", "Description1", 0, "Snippet1", isActive: true);
            var savedEntity1 = repo.AddOrUpdate(entity1);
            var entity2 = new Post(0, 0, "Title2", "Description2", 0, "SnippeTKey", isActive: false);
            _ = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Post>("Title", "Title1"));

            // Act
            var listEntities = repo.ListEntities(filters: filters, status: Status.Active);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Count == 1);
            Assert.IsTrue(savedEntity1.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesBetweenDates()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var date2 = DateTime.Now.AddDays(2);
            var date4 = DateTime.Now.AddDays(4);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", date2, false);
            var entity3 = new Post(0, 1, "Title3", "Description3", 1, "Snippet3", DateTime.Now.AddDays(3), true);
            var entity4 = new Post(0, 2, "Title4", "Description4", 2, "Snippet4", date4, false);
            var entity5 = new Post(0, 2, "Title5", "Description5", 2, "Snippet5", DateTime.Now.AddDays(5), false);
            repo.AddOrUpdate(entity1);
            var savedEntity1 = repo.AddOrUpdate(entity2);
            var savedEntity2 = repo.AddOrUpdate(entity3);
            var savedEntity3 = repo.AddOrUpdate(entity4);
            repo.AddOrUpdate(entity5);
            var filters = ImmutableList.Create(new Filter<Post>("BetweenPublished", (date2, date4)));

            // Act
            var listEntities = repo.ListEntities(filters, 50)
            ;

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Count == 3);
            Assert.IsTrue(savedEntity1.Value.Equals(listEntities.Value[0]));
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value[1]));
            Assert.IsTrue(savedEntity3.Value.Equals(listEntities.Value[2]));
        }

        [TestMethod]
        public void CanListEntitiesByActive()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), false);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Post>("IsActive", true));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByAuthor()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Post>("Author", (uint)2));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByDescription()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Post>("Description", "Description2"));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesById()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Post>("Id", (uint)2));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByIdAndTitle()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new[] { new Filter<Post>("Id", (uint)2), new Filter<Post>("Title", "Title2") });

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByIdAndTitleNotFound()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            repo.AddOrUpdate(entity1);
            repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new[] { new Filter<Post>("Id", (uint)1), new Filter<Post>("Title", "Title2") });

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsFalse(listEntities.Value.Any());
        }

        [TestMethod]
        public void CanListEntitiesByLanguage()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Post>("Language", (uint)2));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByPage()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            var entity3 = new Post(0, 1, "Title3", "Description3", 1, "Snippet3", DateTime.Now.AddDays(3), true);
            var entity4 = new Post(0, 2, "Title4", "Description4", 2, "Snippet4", DateTime.Now.AddDays(4), false);
            var entity5 = new Post(0, 2, "Title5", "Description5", 2, "Snippet5", DateTime.Now.AddDays(5), false);
            repo.AddOrUpdate(entity1);
            var savedEntity1 = repo.AddOrUpdate(entity2);
            var savedEntity2 = repo.AddOrUpdate(entity3);
            repo.AddOrUpdate(entity4);
            repo.AddOrUpdate(entity5);

            // Act
            var listEntities = repo.ListEntities(take: 2, skip: 1)
            ;

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Count == 2);
            Assert.IsTrue(savedEntity1.Value.Equals(listEntities.Value[0]));
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value[1]));
        }

        [TestMethod]
        public void CanListEntitiesBySnippet()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Post>("Snippet", "SnippeTKey"));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByTitle()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), true);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), false);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Post>("Title", "Title2"));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesDesc()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 1, "Title1", "Description1", 1, "Snippet1", DateTime.Now.AddDays(1), false);
            var entity2 = new Post(0, 2, "Title2", "Description2", 2, "SnippeTKey", DateTime.Now.AddDays(2), true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);

            // Act
            var listEntities = repo.ListEntities(desc: true);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesInactive()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqlitePostRepository(conn);
            var entity1 = new Post(0, 0, "Title1", "Description1", 0, "Snippet1", isActive: true);
            _ = repo.AddOrUpdate(entity1);
            var entity2 = new Post(0, 0, "Title2", "Description2", 0, "SnippeTKey", isActive: false);
            var savedEntity2 = repo.AddOrUpdate(entity2);

            // Act
            var listEntities = repo.ListEntities(status: Status.Inactive);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Count == 1);
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanUpdateEntity()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);
            var entity = new Post(0, 1, "Title", "Description", 1, "Snippet1", DateTime.Now, true);
            ;
            var savedEntity = repo.AddOrUpdate(entity);

            // Act
            var updatedEntity = new Post(savedEntity.Value,
                author: 2,
                title: "newTitle",
                description: "newDesciprtion",
                language: 2,
                snippet: "newSnippet",
                published: DateTime.Now.AddDays(1),
                isActive: false);

            var newUpdatedEntity = repo.AddOrUpdate(updatedEntity);

            // Assert
            Assert.IsTrue(newUpdatedEntity.IsValid);
            Assert.IsTrue(newUpdatedEntity.Value.Equals(updatedEntity));
            Assert.IsTrue((newUpdatedEntity.Value as Entity<uint>).Equals(savedEntity.Value));
        }

        [TestMethod]
        public void ListEntitiesIsSafe()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqlitePostRepository(conn);

            // Act
            _conn.Dispose();
            var listEntities = repo.ListEntities();

            // Assert
            Assert.IsFalse(listEntities.IsValid);
        }

        #endregion Public Methods
    }
}