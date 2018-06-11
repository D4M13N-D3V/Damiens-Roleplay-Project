using CitizenFX.Core;
using MySql.Data.MySqlClient;

namespace server.Main
{
    public class DatabaseManager : BaseScript
    {
        public MySqlConnection Connection;

        public static DatabaseManager Instance;



        public DatabaseManager()
        {
            Instance = this;
            Connect();
        }

        private void Connect()
        {
            string connectionString = "server=pirp.site;database=pirpsite_gameserver;user=pirpsite;password=CdZjQ7iHWg";
            Connection = new MySqlConnection(connectionString);
        }

        public MySqlDataReader StartQuery(string query)
        {
            MySqlCommand queryCommand = new MySqlCommand(query, Connection);
            queryCommand.Connection.Open();
            return queryCommand.ExecuteReader(); ;
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
            MySqlCommand queryCommand = new MySqlCommand(query,Connection);
            queryCommand.Connection.Open();
            queryCommand.ExecuteNonQuery();
            queryCommand.Connection.Close();
        }
    }
}
