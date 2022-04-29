using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

using Expense.Infrastructure.Helpers;

using Expenses.Core.Services;

namespace Expense.Infrastructure.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        public async Task<List<Expenses.Core.Domain.Expense>> GetAllAsync()
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var result = await con.QueryAsync<Expenses.Core.Domain.Expense>("select * from Expense", commandType: CommandType.Text);
                return result.ToList();
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int dbResult = await con.ExecuteAsync("delete from Expense where Id = @Id", new { Id = id }, commandType: CommandType.Text);
                return dbResult != 0;
            }
        }

        public async Task<int> InsertAsync(Expenses.Core.Domain.Expense data)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { ItemId = data.Item.Id, data.Reason, data.Quantity, data.UnitCost, CreatedDate = DateTime.UtcNow });
                await con.ExecuteAsync(StoredProcedures.Expense.InsertExpense, dynamicParameters, commandType: CommandType.StoredProcedure);
                return dynamicParameters.Get<int>("@Id");
            };
        }

        public async Task<bool> UpdateAsync(Expenses.Core.Domain.Expense data)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { data.ItemId, data.Reason, data.Quantity, data.UnitCost, UpdatedDate = DateTime.UtcNow });
                var dbResult = await con.ExecuteAsync(StoredProcedures.Expense.UpdateExpense, dynamicParameters, commandType: CommandType.StoredProcedure);

                return dbResult != 0;
            };
        }
    }
}