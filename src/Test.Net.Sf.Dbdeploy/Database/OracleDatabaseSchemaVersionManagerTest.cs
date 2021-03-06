using System;
using Net.Sf.Dbdeploy.Configuration;

namespace Net.Sf.Dbdeploy.Database
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.OracleClient;
    using System.Text;
    using NUnit.Framework;

	[Category("Oracle"), Category("DbIntegration")]
	public class OracleDatabaseSchemaVersionManagerTest : AbstractDatabaseSchemaVersionManagerTest
	{
		private static string _connectionString;
		private const string FOLDER = "Scripts";

		protected override string ConnectionString
		{
		    get
		    {
                if (_connectionString == null)
                {
                    _connectionString = ConfigurationManager.AppSettings["OracleConnString-" + Environment.MachineName]
                                        ?? ConfigurationManager.AppSettings["OracleConnString"];
                }
                
                return _connectionString;
		    }
		}

		protected override string Folder
		{
			get { return FOLDER; }
		}

		protected override string Dbms
		{
			get { return SupportedDbms.ORACLE; }
		}

		protected override void InsertRowIntoTable(int i)
		{
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.AppendFormat("INSERT INTO {0}", TableName);
            commandBuilder.Append("(ChangeId, ScriptNumber, Folder, StartDate, CompleteDate, AppliedBy, ScriptName, ScriptStatus, ScriptOutput)");
            commandBuilder.AppendFormat(" VALUES ('{0}', {1}, '{2}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, USER, 'Unit test', 1, '')", Guid.NewGuid(), i, FOLDER);
			ExecuteSql(commandBuilder.ToString());
		}

		protected override IDbConnection GetConnection()
		{
			return new OracleConnection(_connectionString);
		}

        /// <summary>
        /// Ensures the table does not exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
		protected override void EnsureTableDoesNotExist(string tableName)
		{
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append("Begin");
			commandBuilder.AppendFormat(" execute immediate 'DROP TABLE {0}';", tableName);
			commandBuilder.Append(" Exception when others then null;");
			commandBuilder.Append(" End;");

			ExecuteSql(commandBuilder.ToString());
		}

		[Test]
        public override void ShouldNotThrowExceptionIfAllPreviousScriptsAreCompleted()
		{
			this.EnsureTableDoesNotExist();
			CreateTable();
			InsertRowIntoTable(3);
			var changeNumbers = new List<ChangeEntry>(databaseSchemaVersion.GetAppliedChanges());

			Assert.AreEqual(1, changeNumbers.Count);
			Assert.AreEqual(3, changeNumbers[0].ScriptNumber);
		}

		[Test]
		public override void TestCanRetrieveSchemaVersionFromDatabase()
		{
			base.TestCanRetrieveSchemaVersionFromDatabase();
		}

        [Test]
        public override void TestReturnsNoAppliedChangesWhenDatabaseTableDoesNotExist()
        {
            base.TestReturnsNoAppliedChangesWhenDatabaseTableDoesNotExist();
        }

		[Test]
		public override void TestShouldReturnEmptySetWhenTableHasNoRows()
		{
			base.TestShouldReturnEmptySetWhenTableHasNoRows();
		}

        [Test]
        public override void TestShouldCreateChangeLogTableWhenToldToDoSo()
        {
            base.TestShouldCreateChangeLogTableWhenToldToDoSo();
        }
	}
}