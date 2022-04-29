using System.Configuration;

namespace Expense.Infrastructure.Helpers
{
    public class ConfigHelper
    {
        private const string _connection = "databaseConnection";

        public static string ConnnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[_connection].ConnectionString;
            }
        }
    }
}