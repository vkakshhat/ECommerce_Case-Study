using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Utility_Library
{
    public static class PropertyUtil
    {
        // Method to get the connection string from the appsettings.json
        public static string GetPropertyString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("dbCn");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'dbCn' is not found in appsettings.json.");
            }

            return connectionString;
        }
    }
}
