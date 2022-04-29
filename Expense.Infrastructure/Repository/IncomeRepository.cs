using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

using Expense.Infrastructure.Helpers;

using Expenses.Core.Domain;
using Expenses.Core.Services;

namespace Expense.Infrastructure.Repository
{
    public class IncomeRepository : IIncomeRepository
    {
        public async Task<List<Income>> GetAllAsync()
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var result = await con.QueryAsync<Income>("select * from Income", commandType: CommandType.Text);
                return result.ToList();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int dbResult = await con.ExecuteAsync("delete from Income where Id = @Id", new { Id = id }, commandType: CommandType.Text);
                return dbResult != 0;
            };
        }

        public async Task<int> InsertAsync(Income data)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { data.Name, data.Amount, data.Description, CreatedDate = DateTime.UtcNow });
                await con.ExecuteAsync(StoredProcedures.Income.InsertIncome, dynamicParameters, commandType: CommandType.StoredProcedure);
                return dynamicParameters.Get<int>("@Id");
            };
        }

        public async Task<bool> UpdateAsync(Income data)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var dbResult = await con.ExecuteAsync(StoredProcedures.Income.UpdateIncome, new { data.Id, data.Name, data.Amount, data.Description, UpdatedDate = DateTime.UtcNow }, commandType: CommandType.StoredProcedure);

                return dbResult != 0;
            };
        }
    }
}