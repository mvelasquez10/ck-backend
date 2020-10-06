using System;
using System.Collections.Immutable;
using System.Linq;

using CK.Entities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Repository.SQLite.Tests
{
    [TestClass]
    public class LanguageRepositoryTest : BaseRepositoryTest
    {
        #region Public Methods

        [TestMethod]
        public void AddOrUpdateEntityIsRequired()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqliteLanguageRepository(conn);

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
            var entity = new Language(0, "Name");
            var repo = new SqliteLanguageRepository(conn);

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

            var repo = new SqliteLanguageRepository(conn);
            var entity = new Language(0, "Name");

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
            var repo = new SqliteLanguageRepository(conn);
            var entity = new Language(0, "Name1");
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

            var repo = new SqliteLanguageRepository(conn);
            var entity = new Language(0, "Name");
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

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1", true);
            var savedEntity1 = repo.AddOrUpdate(entity1);
            var entity2 = new Language(0, "Name2", false);
            _ = repo.AddOrUpdate(entity2);

            // Act
            var listEntities = repo.ListEntities(status: Status.Active);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Count == 1);
            Assert.IsTrue(savedEntity1.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesActiveAndByName()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1", true);
            var savedEntity1 = repo.AddOrUpdate(entity1);
            var entity2 = new Language(0, "Name2", false);
            _ = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Language>("Name", "Name1"));

            // Act
            var listEntities = repo.ListEntities(filters: filters, status: Status.Active);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Count == 1);
            Assert.IsTrue(savedEntity1.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByActive()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1", false);
            var entity2 = new Language(1, "Name2", true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Language>("IsActive", true));

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

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1");
            var entity2 = new Language(0, "Name2");
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Language>("Id", (uint)2));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByIdAndName()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1");
            var entity2 = new Language(0, "Name2");
            repo.AddOrUpdate(entity1);
            var savedLanguage2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new[] { new Filter<Language>("Id", (uint)2), new Filter<Language>("Name", "Name2") });

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedLanguage2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByIdAndNameNotFound()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1");
            var entity2 = new Language(0, "Name2");
            repo.AddOrUpdate(entity1);
            repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new[] { new Filter<Language>("Id", (uint)1), new Filter<Language>("Name", "Name2") });

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsFalse(listEntities.Value.Any());
        }

        [TestMethod]
        public void CanListEntitiesByName()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1");
            var entity2 = new Language(0, "Name2");
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<Language>("Name", "Name2"));

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

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1");
            var entity2 = new Language(0, "Name2");
            var entity3 = new Language(0, "Name3");
            var entity4 = new Language(0, "Name4");
            var entity5 = new Language(0, "Name5");
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
        public void CanListEntitiesInactive()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteLanguageRepository(conn);
            var entity1 = new Language(0, "Name1", true);
            _ = repo.AddOrUpdate(entity1);
            var entity2 = new Language(0, "Name2", false);
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

            var repo = new SqliteLanguageRepository(conn);
            var entity = new Language(0, "Name");
            var savedEntity = repo.AddOrUpdate(entity);

            // Act
            var updatedEntity = new Language(savedEntity.Value,
                name: "NewName");

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
            var repo = new SqliteLanguageRepository(conn);

            // Act
            _conn.Dispose();
            var listEntities = repo.ListEntities();

            // Assert
            Assert.IsFalse(listEntities.IsValid);
        }

        [TestMethod]
        public void RepositoryExist()
        {
            // Arrange
            var conn = GetConnectionString();

            // Act
            // Assert
            var repo1 = new SqliteLanguageRepository(conn);
            Assert.IsFalse(repo1.Exist());
            var repo2 = new SqliteLanguageRepository(conn);
            Assert.IsTrue(repo2.Exist());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _conn.Dispose();
        }

        #endregion Public Methods
    }
}