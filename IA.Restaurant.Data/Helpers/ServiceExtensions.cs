using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IA.Restaurant.Data.Helpers
{
    /// <summary>
    /// This is a class with static methods to extend the IApplicationBuilder methods,
    /// IConfiguration, DbContextOptionsBuilder and others as needed
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// get connection string (you can added more providers)
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="dataBase"></param>
        public static string GetConnection(this IConfiguration configuration, string dataBase)
        {
            string connectionString;
            switch (dataBase)
            {
                case Providers.MYSQL:
                    connectionString = configuration.GetConnectionString("MYSQLContext");
                    break;
                case Providers.MSSQL:
                default:
                    connectionString = configuration.GetConnectionString("MSSQLContext");
                    break;
            }
            return connectionString;
        }


        /// <summary>
        /// select which DBMS the EF will use
        /// </summary>
        /// <param name="options"></param>
        /// <param name="connectionString"></param>
        /// <param name="dataBase"></param>
        public static void UseDatabase(this DbContextOptionsBuilder options, string connectionString, string dataBase, int commandTimeOut = 60)
        {
            switch (dataBase)
            {
                case Providers.MYSQL:
                    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 15)),
                                        mySqlOptions =>
                                        {
                                            mySqlOptions.CommandTimeout(commandTimeOut);
                                        });
                    break;
                case Providers.MSSQL:
                default:
                    options.UseSqlServer(connectionString,
                                        sqlServerOptions => sqlServerOptions.CommandTimeout(commandTimeOut));
                    break;
            }
        }
    }
}
