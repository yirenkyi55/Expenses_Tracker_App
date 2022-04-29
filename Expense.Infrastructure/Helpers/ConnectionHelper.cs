using System;
using System.Data;
using System.Data.SqlClient;

namespace Expense.Infrastructure.Helpers
{
    public static class ConnectionHelper
    {
        /// <summary>
        /// A higher order function that helps to make connection to database, runs a function and tear down connection
        /// </summary>
        /// <typeparam name="R">The object to return after running the func</typeparam>
        /// <param name="connString">The Connection string</param>
        /// <param name="func">The function to execute</param>
        /// <returns>The object of type <typeparamref name="R"/>  </returns>
        public static R Connect<R>(string connString, Func<IDbConnection, R> func)
        {
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                return func(connection);
            }
        }
    }
}