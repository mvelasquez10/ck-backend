using System;

using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Repository.SQLite.Tests
{
    [TestClass]
    public class FilterTest
    {
        #region Public Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFilterClauseIsRequierd()
        {
            // Arrange
            // Act
            // Assert
            new Filter<int>(null, new SqliteParameter());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFilterParameterIsRequierd()
        {
            // Arrange
            SqliteParameter temp = null;

            // Act
            // Assert
            new Filter<int>("test", temp);
        }

        #endregion Public Methods
    }
}