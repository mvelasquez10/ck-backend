using System;
using System.Collections.Immutable;
using System.Linq;

using CK.Entities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Repository.SQLite.Tests
{
    [TestClass]
    public class UserRepositoryTest : BaseRepositoryTest
    {
        #region Public Methods

        [TestMethod]
        public void AddOrUpdateEntityIsRequired()
        {
            // Arrange
            var conn = GetConnectionString();
            var repo = new SqliteUserRepository(conn);

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
            var entity = new User(0, "email", new byte[] { 0x1, 0x2 }, "Name", "Surname");
            var repo = new SqliteUserRepository(conn);

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

            var repo = new SqliteUserRepository(conn);
            var entity = new User(0, "email", new byte[] { 0x1, 0x2 }, "Name");

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
            var repo = new SqliteUserRepository(conn);
            var entity = new User(0, "email", new byte[] { 0x1, 0x2 }, "Name");
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
            var repo = new SqliteUserRepository(conn);

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

            var repo = new SqliteUserRepository(conn);
            var entity = new User(0, "email", new byte[] { 0x1, 0x2 }, "Name");
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

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x2 }, "Name1", isActive: true);
            var savedEntity1 = repo.AddOrUpdate(entity1);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x1 }, "Name2", isActive: false);
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

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x2 }, "Name1", isActive: true);
            var savedEntity1 = repo.AddOrUpdate(entity1);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x1 }, "Name2", isActive: false);
            _ = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<User>("Name", "Name1"));

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

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<User>("IsActive", true));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByAdmin()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<User>("IsAdmin", true));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByEmail()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<User>("Email", "email2"));

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

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<User>("Id", (uint)2));

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

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new[] { new Filter<User>("Id", (uint)2), new Filter<User>("Name", "Name2") });

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByIdAndNameNotFound()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new[] { new Filter<User>("Id", (uint)1), new Filter<User>("Name", "Name2") });

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

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<User>("Name", "Name2"));

            // Act
            var listEntities = repo.ListEntities(filters);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.IsTrue(listEntities.Value.Any());
            Assert.IsTrue(savedEntity2.Value.Equals(listEntities.Value.First()));
        }

        [TestMethod]
        public void CanListEntitiesByNameOrSurname()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "NameB", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "SurnameA", true, true);
            var entity3 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name3", "SurnameB", true, true);
            var savedEntity1 = repo.AddOrUpdate(entity1);
            repo.AddOrUpdate(entity2);
            var savedEntity3 = repo.AddOrUpdate(entity3);
            var filters = ImmutableList.Create(new[]
            {
                new Filter<User>("NameOrSurname", ("%b%", "%b%"))
            });

            // Act
            var listEntities = repo.ListEntities(filters, take: 3);

            // Assert
            Assert.IsTrue(listEntities.IsValid);
            Assert.AreEqual(listEntities.Value.Count, 2);
            Assert.IsTrue(savedEntity1.Value.Equals(listEntities.Value[0]));
            Assert.IsTrue(savedEntity3.Value.Equals(listEntities.Value[1]));
        }

        [TestMethod]
        public void CanListEntitiesByNamePartial()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "abcd", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "efgh", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<User>("Name", "ef%"));

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

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1");
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2");
            var entity3 = new User(0, "email3", new byte[] { 0x3, 0x3 }, "Name3");
            var entity4 = new User(0, "email4", new byte[] { 0x4, 0x4 }, "Name4");
            var entity5 = new User(0, "email5", new byte[] { 0x5, 0x5 }, "Name5");
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
        public void CanListEntitiesBySurname()
        {
            // Arrange
            var conn = GetConnectionString();

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x1 }, "Name1", "Surname1", false, false);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x2 }, "Name2", "Surname2", true, true);
            repo.AddOrUpdate(entity1);
            var savedEntity2 = repo.AddOrUpdate(entity2);
            var filters = ImmutableList.Create(new Filter<User>("Surname", "Surname2"));

            // Act
            var listEntities = repo.ListEntities(filters);

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

            var repo = new SqliteUserRepository(conn);
            var entity1 = new User(0, "email1", new byte[] { 0x1, 0x2 }, "Name1", isActive: true);
            _ = repo.AddOrUpdate(entity1);
            var entity2 = new User(0, "email2", new byte[] { 0x2, 0x1 }, "Name2", isActive: false);
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

            var repo = new SqliteUserRepository(conn);
            var entity = new User(0, "email", new byte[] { 0x1, 0x2 }, "Name", "Surname", false, false);
            var savedEntity = repo.AddOrUpdate(entity);

            // Act
            var updatedEntity = new User(savedEntity.Value,
                email: "NewEmail",
                pass: new byte[] { 0x2, 0x1 },
                name: "NewName",
                surname: "NewSurname",
                isActive: true,
                isAdmin: true);

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
            var repo = new SqliteUserRepository(conn);

            // Act
            _conn.Dispose();
            var listEntities = repo.ListEntities();

            // Assert
            Assert.IsFalse(listEntities.IsValid);
        }

        #endregion Public Methods
    }
}