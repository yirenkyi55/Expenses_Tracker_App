using System;
using System.Collections.Generic;
using System.Data;
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
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, con =>
            {
                return con.QueryAsync<Expenses.Core.Domain.Expense>("select * from Expense", commandType: CommandType.Text);
            });

            return result.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async conn =>
            {
                int dbResult = await conn.ExecuteAsync("delete from Expense where Id = @Id", new { Id = id }, commandType: CommandType.Text);
                return dbResult != 0;
            });

            return result;
        }

        public async Task<int> InsertAsync(Expenses.Core.Domain.Expense data)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async con =>
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { ItemId = data.Item.Id, data.Reason, data.Quantity, data.UnitCost, data, CreatedDate = DateTime.UtcNow });
                await con.ExecuteAsync(StoredProcedures.Expense.InsertExpense, dynamicParameters, commandType: CommandType.StoredProcedure);
                return dynamicParameters.Get<int>("@Id");
            });

            return result;
        }

        public async Task<bool> UpdateAsync(Expenses.Core.Domain.Expense data)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async conn =>
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { data.ItemId, data.Reason, data.Quantity, data.UnitCost, UpdatedDate = DateTime.UtcNow });
                var dbResult = await conn.ExecuteAsync(StoredProcedures.Expense.UpdateExpense, dynamicParameters, commandType: CommandType.StoredProcedure);

                return dbResult != 0;
            });

            return result;
        }
    }
}