using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class ConnectionMySQL
    {
        string strConnect = "Server=localhost;Port=3306;Database=tour;Uid=root;Pwd=;";

        //get value from database
        public DataSet SelectData(string sql)
        {
            using(MySqlConnection Connection = new MySqlConnection(strConnect))
            {
                try
                {
                    Connection.Open();

                    MySqlCommand command = new MySqlCommand(sql, Connection);

                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = command;

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    return ds;
                }
                finally
                {

                }
            }
        }

        //ExecuteScalar
        public object ExecuteScalar(string query)
        {
            using (MySqlConnection Connection = new MySqlConnection(strConnect))
            {
                Connection.Open();

                MySqlCommand cmd = new MySqlCommand(query, Connection);
                var row = cmd.ExecuteScalar();

                return row;
            }
        }

        //ExecuteNonQuery
        public void ExecuteNonQuery(string query)
        {
            using (MySqlConnection Connection = new MySqlConnection(strConnect))
            {
                Connection.Open();

                MySqlCommand cmd = new MySqlCommand(query, Connection);
                cmd.ExecuteNonQuery();           
            }
        }

        //ExcuteReader
        public MySqlDataReader ExcuteReader(string query)
        {
            using (MySqlConnection Connection = new MySqlConnection(strConnect))
            {
                Connection.Open();

                MySqlCommand cmd = new MySqlCommand(query, Connection);
                var reader = cmd.ExecuteReader();

                return reader;
            }
        }
    }
}