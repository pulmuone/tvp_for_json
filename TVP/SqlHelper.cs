using Npgsql;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace TVP
{
    public class SqlHelper
    {
        public SqlHelper()
        {

        }

        public static bool ExecuteNonQuery<T>(string connectionString, string query, params NpgsqlParameter[] sqlParam) where T : new()
        {
            bool result = false;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                connection.MapComposite<T>("employee_udt");
                //NpgsqlTransaction tran = connection.BeginTransaction();

                //NpgsqlConnection.MapCompositeGlobally<Employee_udt>("employee_udt"); //ClassName , pg TypeName

                //using (NpgsqlCommand cmdSql = new NpgsqlCommand(query, connection, tran))
                using (NpgsqlCommand cmdSql = new NpgsqlCommand(query, connection))
                {
                    cmdSql.CommandTimeout = 1800; 
                    cmdSql.CommandType = CommandType.StoredProcedure;
                    cmdSql.Parameters.AddRange(sqlParam);
                    try
                    {
                        cmdSql.ExecuteNonQuery();
                        result = true;
                    }
                    catch (NpgsqlException ex)
                    {
                        //tran.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        //tran.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }

                //tran.Commit();  
            }
            return result;
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params NpgsqlParameter[] sqlParam)
        {
            DataSet ds = new DataSet();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmdSql = new NpgsqlCommand(commandText, connection))
                {
                    cmdSql.CommandTimeout = 1800; 
                    cmdSql.CommandType = commandType;
                    cmdSql.Parameters.AddRange(sqlParam);

                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmdSql))
                    {
                        da.Fill(ds);
                    }
                }
            }
            return ds;
        }
    }
}
