using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ApiPhones.Services
{
    public class ConnectionFactory
    {
        private readonly string _connectionString;
        public ConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            try
            {
                var connection = new OracleConnection(_connectionString);
                connection.Open();
                OracleGlobalization info = connection.GetSessionInfo();
                info.TimeZone = "Asia/Novosibirsk";
                connection.SetSessionInfo(info);
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
