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
            queryCommand.Connection.Open();
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
