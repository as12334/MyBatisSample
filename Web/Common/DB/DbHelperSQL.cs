using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LotterySystem.DBUtility;

namespace BuilderDALSQL
{
	public class DbHelperSQL
	{
		private static string connectionString = PubConstant.DBConnectionString;

		private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
		{
			SqlCommand command = DbHelperSQL.BuildQueryCommand(connection, storedProcName, parameters);
			command.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
			return command;
		}

		private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
		{
			SqlCommand result;
			using (SqlCommand command = new SqlCommand(storedProcName, connection))
			{
				command.CommandType = CommandType.StoredProcedure;
				for (int i = 0; i < parameters.Length; i++)
				{
					SqlParameter parameter = (SqlParameter)parameters[i];
					if (parameter != null)
					{
						if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && parameter.Value == null)
						{
							parameter.Value = DBNull.Value;
						}
						command.Parameters.Add(parameter);
					}
				}
				result = command;
			}
			return result;
		}

		public static bool ColumnExists(string tableName, string columnName)
		{
			object single = DbHelperSQL.GetSingle(string.Concat(new string[]
			{
				"select count(1) from syscolumns where [id]=object_id('",
				tableName,
				"') and [name]='",
				columnName,
				"'"
			}));
			return single != null && Convert.ToInt32(single) > 0;
		}

		public static SqlDataReader ExecuteReader(string strSQL)
		{
			SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString);
			SqlCommand command = new SqlCommand(strSQL, connection)
			{
				CommandTimeout = 1000
			};
			SqlDataReader reader2;
			try
			{
				connection.Open();
				reader2 = command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (SqlException exception)
			{
				throw exception;
			}
			return reader2;
		}

		public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
		{
			SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString);
			SqlCommand cmd = new SqlCommand();
			SqlDataReader reader2;
			try
			{
				DbHelperSQL.PrepareCommand(cmd, conn, null, SQLString, cmdParms);
				SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				reader2 = reader;
			}
			catch (SqlException exception)
			{
				throw exception;
			}
			return reader2;
		}

		public static int ExecuteSql(string SQLString)
		{
			int result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				using (SqlCommand command = new SqlCommand(SQLString, connection))
				{
					try
					{
						connection.Open();
						result = command.ExecuteNonQuery();
					}
					catch (SqlException exception)
					{
						connection.Close();
						throw exception;
					}
				}
			}
			return result;
		}

		public static int executte_sql(string SQLString, string content)
		{
			int num2;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				SqlCommand command = new SqlCommand(SQLString, connection);
				SqlParameter parameter = new SqlParameter("@content", SqlDbType.NText)
				{
					Value = content
				};
				command.Parameters.Add(parameter);
				try
				{
					connection.Open();
					num2 = command.ExecuteNonQuery();
				}
				catch (SqlException exception)
				{
					throw exception;
				}
				finally
				{
					command.Dispose();
					connection.Close();
				}
			}
			return num2;
		}

		public static int executte_sql(string SQLString, params SqlParameter[] cmdParms)
		{
			int result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					try
					{
						DbHelperSQL.PrepareCommand(cmd, connection, null, SQLString, cmdParms);
						int num = cmd.ExecuteNonQuery();
						cmd.Parameters.Clear();
						result = num;
					}
					catch (SqlException exception)
					{
						throw exception;
					}
				}
			}
			return result;
		}

		public static int ExecuteSqlByTime(string SQLString, int Times)
		{
			int result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				using (SqlCommand command = new SqlCommand(SQLString, connection))
				{
					try
					{
						connection.Open();
						command.CommandTimeout = Times;
						result = command.ExecuteNonQuery();
					}
					catch (SqlException exception)
					{
						connection.Close();
						throw exception;
					}
				}
			}
			return result;
		}

		public static object ExecuteSqlGet(string SQLString, string content)
		{
			object result;
			object obj3;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				SqlCommand command = new SqlCommand(SQLString, connection);
				SqlParameter parameter = new SqlParameter("@content", SqlDbType.NText)
				{
					Value = content
				};
				command.Parameters.Add(parameter);
				try
				{
					connection.Open();
					object objA = command.ExecuteScalar();
					if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
					{
						result = null;
						return result;
					}
					obj3 = objA;
				}
				catch (SqlException exception)
				{
					throw exception;
				}
				finally
				{
					command.Dispose();
					connection.Close();
				}
			}
			result = obj3;
			return result;
		}

		public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
		{
			int num2;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				SqlCommand command = new SqlCommand(strSQL, connection);
				SqlParameter parameter = new SqlParameter("@fs", SqlDbType.Image)
				{
					Value = fs
				};
				command.Parameters.Add(parameter);
				try
				{
					connection.Open();
					num2 = command.ExecuteNonQuery();
				}
				catch (SqlException exception)
				{
					throw exception;
				}
				finally
				{
					command.Dispose();
					connection.Close();
				}
			}
			return num2;
		}

		public static int ExecuteSqlTran(List<CommandInfo> cmdList)
		{
			int result;
			int num3;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						int num = 0;
						foreach (CommandInfo info in cmdList)
						{
							string commandText = info.CommandText;
							SqlParameter[] parameters = (SqlParameter[])info.Parameters;
							DbHelperSQL.PrepareCommand(cmd, connection, transaction, commandText, parameters);
							if (info.EffentNextType == EffentNextType.WhenHaveContine || info.EffentNextType == EffentNextType.WhenNoHaveContine)
							{
								if (info.CommandText.ToLower().IndexOf("count(") == -1)
								{
									transaction.Rollback();
									result = 0;
									return result;
								}
								object obj2 = cmd.ExecuteScalar();
								if (obj2 == null && obj2 == DBNull.Value)
								{
								}
								bool flag = Convert.ToInt32(obj2) > 0;
								if (info.EffentNextType == EffentNextType.WhenHaveContine && !flag)
								{
									transaction.Rollback();
									result = 0;
									return result;
								}
								if (info.EffentNextType == EffentNextType.WhenNoHaveContine && flag)
								{
									transaction.Rollback();
									result = 0;
									return result;
								}
							}
							else
							{
								int num2 = cmd.ExecuteNonQuery();
								num += num2;
								if (info.EffentNextType == EffentNextType.ExcuteEffectRows && num2 == 0)
								{
									transaction.Rollback();
									result = 0;
									return result;
								}
								cmd.Parameters.Clear();
							}
						}
						transaction.Commit();
						num3 = num;
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
			result = num3;
			return result;
		}

		public static int ExecuteSqlTran(List<string> SQLStringList)
		{
			int num2;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand
				{
					Connection = connection
				};
				SqlTransaction transaction = connection.BeginTransaction();
				command.Transaction = transaction;
				try
				{
					int num = 0;
					for (int i = 0; i < SQLStringList.Count; i++)
					{
						string str = SQLStringList[i];
						if (str.Trim().Length > 1)
						{
							command.CommandText = str;
							num += command.ExecuteNonQuery();
						}
					}
					transaction.Commit();
					num2 = num;
				}
				catch
				{
					transaction.Rollback();
					num2 = 0;
				}
			}
			return num2;
		}

		public static void ExecuteSqlTran(Hashtable SQLStringList)
		{
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						foreach (DictionaryEntry entry in SQLStringList)
						{
							string cmdText = entry.Key.ToString();
							SqlParameter[] cmdParms = (SqlParameter[])entry.Value;
							DbHelperSQL.PrepareCommand(cmd, connection, transaction, cmdText, cmdParms);
							int num = cmd.ExecuteNonQuery();
							cmd.Parameters.Clear();
						}
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		public static void ExecuteSqlTranWithIndentity(List<CommandInfo> SQLStringList)
		{
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						int num = 0;
						foreach (CommandInfo info in SQLStringList)
						{
							string commandText = info.CommandText;
							SqlParameter[] parameters = (SqlParameter[])info.Parameters;
							SqlParameter[] array = parameters;
							for (int i = 0; i < array.Length; i++)
							{
								SqlParameter parameter = array[i];
								if (parameter.Direction == ParameterDirection.InputOutput)
								{
									parameter.Value = num;
								}
							}
							DbHelperSQL.PrepareCommand(cmd, connection, transaction, commandText, parameters);
							int num2 = cmd.ExecuteNonQuery();
							array = parameters;
							for (int i = 0; i < array.Length; i++)
							{
								SqlParameter parameter = array[i];
								if (parameter.Direction == ParameterDirection.Output)
								{
									num = Convert.ToInt32(parameter.Value);
								}
							}
							cmd.Parameters.Clear();
						}
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		public static void ExecuteSqlTranWithIndentity(Hashtable SQLStringList)
		{
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						int num = 0;
						foreach (DictionaryEntry entry in SQLStringList)
						{
							string cmdText = entry.Key.ToString();
							SqlParameter[] cmdParms = (SqlParameter[])entry.Value;
							SqlParameter[] array = cmdParms;
							for (int i = 0; i < array.Length; i++)
							{
								SqlParameter parameter = array[i];
								if (parameter.Direction == ParameterDirection.InputOutput)
								{
									parameter.Value = num;
								}
							}
							DbHelperSQL.PrepareCommand(cmd, connection, transaction, cmdText, cmdParms);
							int num2 = cmd.ExecuteNonQuery();
							array = cmdParms;
							for (int i = 0; i < array.Length; i++)
							{
								SqlParameter parameter = array[i];
								if (parameter.Direction == ParameterDirection.Output)
								{
									num = Convert.ToInt32(parameter.Value);
								}
							}
							cmd.Parameters.Clear();
						}
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		public static bool Exists(string strSql)
		{
			object single = DbHelperSQL.GetSingle(strSql);
			int num;
			if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
			{
				num = 0;
			}
			else
			{
				num = int.Parse(single.ToString());
			}
			return num != 0;
		}

		public static bool Exists(string strSql, params SqlParameter[] cmdParms)
		{
			object single = DbHelperSQL.GetSingle(strSql, cmdParms);
			int num;
			if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
			{
				num = 0;
			}
			else
			{
				num = int.Parse(single.ToString());
			}
			return num != 0;
		}

		public static int GetMaxID(string FieldName, string TableName)
		{
			object single = DbHelperSQL.GetSingle("select max(" + FieldName + ")+1 from " + TableName);
			int result;
			if (single == null)
			{
				result = 1;
			}
			else
			{
				result = int.Parse(single.ToString());
			}
			return result;
		}

		public static object GetSingle(string SQLString)
		{
			object result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				using (SqlCommand command = new SqlCommand(SQLString, connection))
				{
					try
					{
						connection.Open();
						object objA = command.ExecuteScalar();
						if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
						{
							result = null;
						}
						else
						{
							result = objA;
						}
					}
					catch (SqlException exception)
					{
						connection.Close();
						throw exception;
					}
				}
			}
			return result;
		}

		public static object GetSingle(string SQLString, int Times)
		{
			object result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				using (SqlCommand command = new SqlCommand(SQLString, connection))
				{
					try
					{
						connection.Open();
						command.CommandTimeout = Times;
						object objA = command.ExecuteScalar();
						if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
						{
							result = null;
						}
						else
						{
							result = objA;
						}
					}
					catch (SqlException exception)
					{
						connection.Close();
						throw exception;
					}
				}
			}
			return result;
		}

		public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
		{
			object result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					try
					{
						DbHelperSQL.PrepareCommand(cmd, connection, null, SQLString, cmdParms);
						object objA = cmd.ExecuteScalar();
						cmd.Parameters.Clear();
						if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
						{
							result = null;
						}
						else
						{
							result = objA;
						}
					}
					catch (SqlException exception)
					{
						throw exception;
					}
				}
			}
			return result;
		}

		private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
		{
			if (conn.State != ConnectionState.Open)
			{
				conn.Open();
			}
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
			{
				cmd.Transaction = trans;
			}
			cmd.CommandType = CommandType.Text;
			if (cmdParms != null)
			{
				for (int i = 0; i < cmdParms.Length; i++)
				{
					SqlParameter parameter = cmdParms[i];
					if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && parameter.Value == null)
					{
						parameter.Value = DBNull.Value;
					}
					cmd.Parameters.Add(parameter);
				}
			}
		}

		public static DataSet Query(string SQLString)
		{
			DataSet set2;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				DataSet dataSet = new DataSet();
				try
				{
					connection.Open();
					new SqlDataAdapter(SQLString, connection).Fill(dataSet, "ds");
					set2 = dataSet;
				}
				catch (SqlException exception)
				{
					throw new Exception(exception.Message);
				}
			}
			return set2;
		}

		public static DataSet Query(string SQLString, int Times)
		{
			DataSet result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				DataSet dataSet = new DataSet();
				try
				{
					connection.Open();
					new SqlDataAdapter(SQLString, connection)
					{
						SelectCommand = 
						{
							CommandTimeout = Times
						}
					}.Fill(dataSet, "ds");
				}
				catch (SqlException exception)
				{
					throw new Exception(exception.Message);
				}
				result = dataSet;
			}
			return result;
		}

		public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
		{
			DataSet set2;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				SqlCommand cmd = new SqlCommand();
				DbHelperSQL.PrepareCommand(cmd, connection, null, SQLString, cmdParms);
				using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
				{
					DataSet dataSet = new DataSet();
					try
					{
						adapter.Fill(dataSet, "ds");
						cmd.Parameters.Clear();
					}
					catch (SqlException exception)
					{
						throw new Exception(exception.Message);
					}
					set2 = dataSet;
				}
			}
			return set2;
		}

		public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
		{
			SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString);
			connection.Open();
			SqlCommand command = DbHelperSQL.BuildQueryCommand(connection, storedProcName, parameters);
			command.CommandType = CommandType.StoredProcedure;
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
		{
			DataSet result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				new SqlDataAdapter
				{
					SelectCommand = DbHelperSQL.BuildQueryCommand(connection, storedProcName, parameters)
				}.Fill(dataSet, tableName);
				connection.Close();
				result = dataSet;
			}
			return result;
		}

		public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
		{
			int result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				connection.Open();
				SqlCommand command = DbHelperSQL.BuildIntCommand(connection, storedProcName, parameters);
				rowsAffected = command.ExecuteNonQuery();
				result = (int)command.Parameters["ReturnValue"].Value;
			}
			return result;
		}

		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
		{
			DataSet result;
			using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				SqlDataAdapter adapter = new SqlDataAdapter
				{
					SelectCommand = DbHelperSQL.BuildQueryCommand(connection, storedProcName, parameters)
				};
				adapter.SelectCommand.CommandTimeout = Times;
				adapter.Fill(dataSet, tableName);
				connection.Close();
				result = dataSet;
			}
			return result;
		}

		public static bool TabExists(string TableName)
		{
			object single = DbHelperSQL.GetSingle("select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1");
			int num;
			if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
			{
				num = 0;
			}
			else
			{
				num = int.Parse(single.ToString());
			}
			return num != 0;
		}
	}
}
