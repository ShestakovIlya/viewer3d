using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace viewer3d
{
    /* Класс для работы  с базой SQL Server 2019 Express */
    public class DBMSServer : IDataBase
    {
        /* Определение строки подключения к БД */
        private static string _connectionValue = null;
        protected static string ConnectionValue
        {
            get {
                if(_connectionValue is null)
                {                    
                    _connectionValue = ConfigurationManager.ConnectionStrings["connect_database"]?.ConnectionString ??
                        @"Data Source =.\SQLEXPRESS; Initial Catalog = viewer3d; Integrated Security=SSPI";
                }
                return _connectionValue; 
            }
        }
       
        /* Чтение информации из БД*/
        public DataSet RequestToData(string sql)
        {
            DataSet _data_set = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionValue))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    adapter.Fill(_data_set);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return _data_set;
        }

        /* Запись информации в БД */
        public int Insert(string sql)
        {
            int ItemID = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionValue))
            {               
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = sql;
                    ItemID = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return ItemID;
        }
    }
}
