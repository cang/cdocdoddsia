using System;
using System.Data;
using System.Data.OleDb;

namespace SIA.Workbench.Utilities
{
	/// <summary>
	/// Summary description for Database.
	/// </summary>
	internal class ScanFolderDatabase
	{
		/// <summary>
		/// Connection string
		/// </summary>
		private string _connectionString;

		/// <summary>
		/// Initialize database for scan folder engine
		/// </summary>
		/// <param name="dbFileName"></param>
		public ScanFolderDatabase(string dbFileName)
		{
			_connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", dbFileName);
		}

		/// <summary>
		/// Check if the file is existed in database
		/// </summary>
		/// <param name="filename">The file to be checked</param>
		/// <returns>true if the file is existed</returns>
		public bool IsExisted(string filename)
		{
			OleDbConnection connection = null;
			OleDbCommand cmd = null;

			try
			{
				connection = CreateConnection();
				cmd = connection.CreateCommand();
				cmd.CommandText = "IsExisted";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@FileName", filename);
				return cmd.ExecuteScalar() != null;
			}
			finally
			{
				if ( cmd != null )
				{
					cmd.Dispose();
					cmd = null;
				}
				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
					connection = null;
				}
			}
		}

		/// <summary>
		/// Insert the file name into database
		/// </summary>
		/// <param name="filename">The file name to be inserted</param>
		/// <returns>true if successfully</returns>
		public bool Insert(string filename)
		{
			OleDbConnection connection = null;
			OleDbCommand cmd = null;

			try
			{
				connection = CreateConnection();
				cmd = connection.CreateCommand();
				cmd.CommandText = "[Insert]";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@FileName", filename);
				return cmd.ExecuteNonQuery() != 0;
			}
			finally
			{
				if ( cmd != null )
				{
					cmd.Dispose();
					cmd = null;
				}
				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
					connection = null;
				}
			}
		}

		/// <summary>
		/// Delete old files if they're older input date
		/// </summary>
		/// <param name="processedDate">Input date</param>
		public void DeleteByProcessedDate(DateTime processedDate)
		{
			OleDbConnection connection = null;
			OleDbCommand cmd = null;

			try
			{
				connection = CreateConnection();
				cmd = connection.CreateCommand();
				cmd.CommandText = "DeleteByProcessedDate";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@ProcessedDate", processedDate);
				cmd.ExecuteNonQuery();
			}
			finally
			{
				if ( cmd != null )
				{
					cmd.Dispose();
					cmd = null;
				}
				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
					connection = null;
				}
			}
		}

		/// <summary>
		/// Delete file from database
		/// </summary>
		/// <param name="filename">The file to be deleted</param>
		public void DeleteByFileName(string filename)
		{
			OleDbConnection connection = null;
			OleDbCommand cmd = null;

			try
			{
				connection = CreateConnection();
				cmd = connection.CreateCommand();
				cmd.CommandText = "DeleteByFileName";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@FileName", filename);
				cmd.ExecuteNonQuery();
			}
			finally
			{
				if ( cmd != null )
				{
					cmd.Dispose();
					cmd = null;
				}
				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
					connection = null;
				}
			}
		}

		/// <summary>
		/// Clear database
		/// </summary>
		public void ClearDatabase()
		{
			OleDbConnection connection = null;
			OleDbCommand cmd = null;

			try
			{
				connection = CreateConnection();
				cmd = connection.CreateCommand();
				cmd.CommandText = "ClearAll";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.ExecuteNonQuery();
			}
			finally
			{
				if ( cmd != null )
				{
					cmd.Dispose();
					cmd = null;
				}
				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
					connection = null;
				}
			}
		}

		private OleDbConnection CreateConnection()
		{
			OleDbConnection connection = new OleDbConnection(_connectionString);
			connection.Open();
			return connection;
		}
	}
}
