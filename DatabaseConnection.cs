using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string path)
        {
            _connectionString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};", path);
        }

        public int ExecuteUpdate(string sql, params object[] arguments)
        {
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            {
                connection.Open();
                foreach (object argument in arguments)
                {
                    command.Parameters.Add(new OleDbParameter("", argument));
                }
                return command.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteQuery(string sql, params object[] arguments)
        {
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            {
                connection.Open();
                foreach (object argument in arguments)
                {
                    command.Parameters.Add(argument);
                }
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
        }
    }
}
