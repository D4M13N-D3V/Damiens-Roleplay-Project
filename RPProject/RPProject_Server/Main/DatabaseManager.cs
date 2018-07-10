using System.Data;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MySql.Data.MySqlClient;

namespace server.Main
{
    public class DatabaseManager : BaseScript
    {
        public MySqlConnection Connection;

        private MySqlDataReader last;

        public static DatabaseManager Instance;

        public DatabaseManager()
        {
            Instance = this;
            Connect();
        }

        private void Connect()
        {
            string connectionString = "server="+API.GetConvar("sql_ip","localhost")+";database=pirpsite_gameserver;user="+API.GetConvar("sql_user", "root")+";password=" + API.GetConvar("sql_pass", "");
            Connection = new MySqlConnection(connectionString);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<MySqlDataReader> StartQuery(string query)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (Connection.State == ConnectionState.Open)
            {
                EndQuery(last);
                last = null;
            }
            MySqlCommand queryCommand = new MySqlCommand(query, Connection);
            queryCommand.Connection.Open();
            last = queryCommand.ExecuteReader();
            return last;
        }

        public void EndQuery(MySqlDataReader reader)
        {
            reader.Close();
            Connection.Close();
        }

        public object Scalar(string query)
        {   
            MySqlCommand queryCommand = new MySqlCommand(query, Connection);
            queryCommand.Connection.OpenAsync();
            var ret = queryCommand.ExecuteScalar();
            Connection.Close();
            return ret;
        } 

        public void Execute(string query)
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
            MySqlCommand queryCommand = new MySqlCommand(query,Connection);
            queryCommand.Connection.Open();
            queryCommand.ExecuteNonQuery();
            queryCommand.Connection.Close();
        }
    }
}
