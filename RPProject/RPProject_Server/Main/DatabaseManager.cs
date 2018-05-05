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
            //Setup Database Connection
            string connectionString = "server=pirp.site;database=pirpsite_gameserver;user=pirpsite;password=CdZjQ7iHWg";
            Connection = new MySqlConnection(connectionString);
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

        public MySqlDataReader StartQuery(string query)
        {
            MySqlCommand queryCommand = new MySqlCommand("SELECT * FROM ITEMS", Connection);
            queryCommand.Connection.Open();
            MySqlDataReader data;
            data = queryCommand.ExecuteReader();
            return data;
        }

        public void EndQuery(MySqlDataReader reader)
        {
            reader.Close();
            Connection.Close();
        }

    }
}
