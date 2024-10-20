using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Utility_Library
{
    public static class DBConnection
    {
        private static readonly string connectionString = PropertyUtil.GetPropertyString();
        private static SqlConnection connection;

        // Static constructor to initialize the connection
        static DBConnection()
        {
            connection = new SqlConnection(connectionString);
        }

        // Method to get the SQL connection
        public static SqlConnection GetConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }

        // Method to close the connection
        public static void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
