using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CK.Repository.SQLite.Tests
{
    public class BaseRepositoryTest
    {
        #region Protected Fields

        protected SqliteConnection _conn;

        #endregion Protected Fields

        #region Public Properties

        public TestContext TestContext { get; set; }

        #endregion Public Properties

        #region Public Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _conn = new SqliteConnection(GetConnectionString());
            _conn.Open();
        }

        #endregion Public Methods

        #region Protected Methods

        protected string GetConnectionString()
        {
            return $"Data Source={TestContext.FullyQualifiedTestClassName}.{TestContext.TestName};Mode=Memory;Cache=Shared";
        }

        #endregion Protected Methods
    }
}