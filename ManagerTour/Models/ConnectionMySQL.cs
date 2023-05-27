using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Tour.Models
{
    public class ConnectionMySQL
    {
        string strConnect = "Server=localhost;Port=3306;Database=tour;Uid=root;Pwd=;";

        public MySqlConnection ConnectionSql()
        {
            MySqlConnection Connection = new MySqlConnection(strConnect);

            try
            {
                Connection.Open();

                return Connection;
            }
            finally
            {
                Connection.Close();
            }
        }

        public DataSet SelectData(string sql)
        {
            MySqlConnection Connection = new MySqlConnection(strConnect);
            Connection = ConnectionSql();

            MySqlCommand command = new MySqlCommand(sql, Connection);
            command.Connection = Connection;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }
    }
}