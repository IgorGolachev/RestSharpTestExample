using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SimpleImpersonation;
using NLog;

namespace core
{
    public sealed class DBHelper
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private static string username = ConfigHelper.DBUserName;
        private static string password = ConfigHelper.DBUserPassword;

        private const string DEV_DOMAIN = "DEVDOMAIN";
        private const string TEST_DOMAIN = "TESTDOMAIN";

        private const string DEV = "Server=DevDB.info;" +
            "Database=TestApp;Trusted_Connection=true;Integrated Security=True;";

        private const string TEST = "Server=TestDB.org;" +
            "Database=TestApp;Trusted_Connection=true;Integrated Security=True";

        private static string GetUserDomain()
        {
            if (ConfigHelper.ExecutionEnvironment.ToUpper().Equals("TEST"))
            {
                return TEST_DOMAIN;
            }
            else
            {
                return DEV_DOMAIN;
            }
        }

        private static string GetConnectionString()
        {
            if (ConfigHelper.ExecutionEnvironment.ToUpper().Equals("TEST"))
            {
                return TEST;
            }
            else
            {
                return DEV;
            }
        }

        public DataTable ExecuteSQLQueryWithResult(string query, Dictionary<string, object> paramList = null)
        {
            DataTable dt = new DataTable();
            var credentials = new UserCredentials(GetUserDomain(), username, password);
            var result = Impersonation.RunAsUser(credentials, LogonType.NewCredentials, () =>
            {
                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                    {
                        ConnectionString = GetConnectionString()
                    };

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            if (paramList != null)
                            {
                                foreach (KeyValuePair<string, object> param in paramList)
                                {
                                    command.Parameters.AddWithValue(param.Key, param.Value);
                                }
                            }

                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    logger.Info(e.Message);
                }
                return dt;
            });

            if (result.Rows.Count.Equals(0))
            {
                throw new NoTestDataFoundException("No suitable test data found");
            }

            return result;
        }
    }
}