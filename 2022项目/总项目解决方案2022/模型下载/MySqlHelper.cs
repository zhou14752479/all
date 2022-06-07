using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 模型下载
{

		// Token: 0x02000041 RID: 65
		public abstract class MySqlHelper
		{
			// Token: 0x060001DA RID: 474 RVA: 0x00009854 File Offset: 0x00007A54
			public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
			{
				MySqlCommand mySqlCommand = new MySqlCommand();
				int result;
				using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
				{
					MySqlHelper.PrepareCommand(mySqlCommand, mySqlConnection, null, cmdType, cmdText, commandParameters);
					int num = mySqlCommand.ExecuteNonQuery();
					mySqlCommand.Parameters.Clear();
					result = num;
				}
				return result;
			}

			// Token: 0x060001DB RID: 475 RVA: 0x000098B0 File Offset: 0x00007AB0
			public static int ExecuteNonQuery(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
			{
				int result;
				try
				{
					MySqlCommand mySqlCommand = new MySqlCommand();
					MySqlHelper.PrepareCommand(mySqlCommand, connection, null, cmdType, cmdText, commandParameters);
					int num = mySqlCommand.ExecuteNonQuery();
					mySqlCommand.Parameters.Clear();
					result = num;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}

			// Token: 0x060001DC RID: 476 RVA: 0x00009900 File Offset: 0x00007B00
			public static int ExecuteNonQuery(MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
			{
				int result;
				try
				{
					MySqlCommand mySqlCommand = new MySqlCommand();
					MySqlHelper.PrepareCommand(mySqlCommand, trans.Connection, trans, cmdType, cmdText, commandParameters);
					int num = mySqlCommand.ExecuteNonQuery();
					mySqlCommand.Parameters.Clear();
					result = num;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}

			// Token: 0x060001DD RID: 477 RVA: 0x00009954 File Offset: 0x00007B54
			public static MySqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
			{
				MySqlCommand mySqlCommand = new MySqlCommand();
				MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
				MySqlDataReader result;
				try
				{
					MySqlHelper.PrepareCommand(mySqlCommand, mySqlConnection, null, cmdType, cmdText, commandParameters);
					MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
					mySqlCommand.Parameters.Clear();
					result = mySqlDataReader;
				}
				catch (Exception ex)
				{
					mySqlConnection.Close();
					throw ex;
				}
				return result;
			}

			// Token: 0x060001DE RID: 478 RVA: 0x000099B4 File Offset: 0x00007BB4
			public static DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
			{
				MySqlCommand mySqlCommand = new MySqlCommand();
				MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
				DataSet result;
				try
				{
					MySqlHelper.PrepareCommand(mySqlCommand, mySqlConnection, null, cmdType, cmdText, commandParameters);
					MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
					mySqlDataAdapter.SelectCommand = mySqlCommand;
					DataSet dataSet = new DataSet();
					mySqlDataAdapter.Fill(dataSet);
					mySqlCommand.Parameters.Clear();
					mySqlConnection.Close();
					result = dataSet;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}

			// Token: 0x060001DF RID: 479 RVA: 0x00009A28 File Offset: 0x00007C28
			public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
			{
				MySqlCommand mySqlCommand = new MySqlCommand();
				object result;
				using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
				{
					try
					{
						MySqlHelper.PrepareCommand(mySqlCommand, mySqlConnection, null, cmdType, cmdText, commandParameters);
						object obj = mySqlCommand.ExecuteScalar();
						mySqlCommand.Parameters.Clear();
						result = obj;
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
				return result;
			}

			// Token: 0x060001E0 RID: 480 RVA: 0x00009A98 File Offset: 0x00007C98
			public static object ExecuteScalar(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
			{
				object result;
				try
				{
					MySqlCommand mySqlCommand = new MySqlCommand();
					MySqlHelper.PrepareCommand(mySqlCommand, connection, null, cmdType, cmdText, commandParameters);
					object obj = mySqlCommand.ExecuteScalar();
					mySqlCommand.Parameters.Clear();
					result = obj;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}

			// Token: 0x060001E1 RID: 481 RVA: 0x00009AE8 File Offset: 0x00007CE8
			public static object ExecuteNonExist(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
			{
				MySqlCommand mySqlCommand = new MySqlCommand();
				object result;
				using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
				{
					MySqlHelper.PrepareCommand(mySqlCommand, mySqlConnection, null, cmdType, cmdText, commandParameters);
					object obj = mySqlCommand.ExecuteNonQuery();
					result = mySqlCommand.LastInsertedId;
				}
				return result;
			}

			// Token: 0x060001E2 RID: 482 RVA: 0x00009B48 File Offset: 0x00007D48
			public static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
			{
				bool flag = conn.State != ConnectionState.Open;
				if (flag)
				{
					conn.Open();
				}
				cmd.Connection = conn;
				cmd.CommandText = cmdText;
				bool flag2 = trans != null;
				if (flag2)
				{
					cmd.Transaction = trans;
				}
				cmd.CommandType = cmdType;
				bool flag3 = cmdParms != null;
				if (flag3)
				{
					foreach (MySqlParameter value in cmdParms)
					{
						cmd.Parameters.Add(value);
					}
				}
			}

			// Token: 0x04000147 RID: 327
			public static string Conn3 = string.Concat(new string[]
			{
			"Database='",
			ClassModel.Most,
			"';Data Source='",
			ClassModel.BelleID,
			"';Port=",
			ClassModel.Project,
			";User Id='",
			ClassModel.Tesource,
			"';Password='",
			ClassModel.Human,
			"';charset='utf8mb3';pooling=true; Allow User Variables=True;"
			});

			// Token: 0x04000148 RID: 328
			public static string Conn = string.Concat(new string[]
			{
			"Database='",
			ClassModel.Most,
			"';Data Source='",
			ClassModel.BelleID,
			"';Port=",
			ClassModel.Project,
			";User Id='",
			ClassModel.Tesource,
			"';Password='",
			ClassModel.Human,
			"';charset='utf8mb4';pooling=true; Allow User Variables=True;"
			});






		public static void login()
		{

            try
            {
				string cmdText = "SELECT value_text FROM ds_setting WHERE setting = 'QQ'";
				
				MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, cmdText, null);
				if (mySqlDataReader.Read())
				{
					if (mySqlDataReader.HasRows)
					{
						int _user_id = mySqlDataReader.GetInt32(0);
						MessageBox.Show(_user_id.ToString());
					}

				}
			}
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
		}







	}
	}



