using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using MySql.Data.MySqlClient;
using roleplay.Main;

namespace roleplay.Main
{
    public class DatabaseManager : BaseScript
    {
        public MySqlConnection Connection;

        private static DatabaseManager instance;

        public DatabaseManager()
        {
            Connect();
        }

        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseManager();
                }
                return instance;
            }
        }

        private void Connect()
        {
            string connectionString = "server=pirp.site;database=pirpsite_gameserver;user=pirpsite;password=CdZjQ7iHWg";
            Connection = new MySqlConnection(connectionString);
        }

        public MySqlDataReader StartQuery(string query)
        {
            MySqlCommand queryCommand = new MySqlCommand("SELECT * FROM ITEMS", Connection);
            queryCommand.Connection.Open();
            return queryCommand.ExecuteReader(); ;
        }

        public void EndQuery(MySqlDataReader reader)
        {
            reader.Close();
            Connection.Close();
        }

        public object StartScalar(string query)
        {
            MySqlCommand queryCommand = new MySqlCommand("SELECT * FROM ITEMS", Connection);
            queryCommand.Connection.Open();
            return queryCommand.ExecuteScalar(); ;
        }

        public void EndScalar()
        {
            Connection.Close();
        }


    }
}
