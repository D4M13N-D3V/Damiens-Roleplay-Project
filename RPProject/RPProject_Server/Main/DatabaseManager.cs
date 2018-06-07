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

        public Task<MySqlDataReader> StartQueryAsync(string query)
        {
            return Task.Run(() =>
            {
                MySqlCommand queryCommand = new MySqlCommand(query, Connection);
                queryCommand.Connection.Open();
                return queryCommand.ExecuteReader();
            });
        }

        public Task EndQueryAsync(MySqlDataReader reader)
        {
            return Task.Run(() =>
            {
                reader.Close();
                Connection.Close();
            });
        }

        public Task<object> Scalar(string query)
        {

            return Task.Run(() =>
            {
                MySqlCommand queryCommand = new MySqlCommand(query, Connection);
                queryCommand.Connection.OpenAsync();
                var ret = queryCommand.ExecuteScalar();
                Connection.Close();
                return ret;
            });
        }

        public Task ExecuteAsync(string query)
        {
            return Task.Run(() =>
            {
                MySqlCommand queryCommand = new MySqlCommand(query, Connection);
                queryCommand.Connection.Open();
                queryCommand.ExecuteNonQuery();
                queryCommand.Connection.Close();
            });
        }
    }
}
