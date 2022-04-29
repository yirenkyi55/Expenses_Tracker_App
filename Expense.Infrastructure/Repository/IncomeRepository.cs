﻿using System;
using System.Collections.Generic;
using System.Data;
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
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, con =>
            {
                return con.QueryAsync<Income>("select * from Income", commandType: CommandType.Text);
            });

            return result.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async conn =>
            {
                int dbResult = await conn.ExecuteAsync("delete from Income where Id = @Id", new { Id = id }, commandType: CommandType.Text);
                return dbResult != 0;
            });

            return result;
        }

        public async Task<int> InsertAsync(Income data)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async con =>
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { data.Name, data.Amount, data.Description, CreatedDate = DateTime.UtcNow });
                await con.ExecuteAsync(StoredProcedures.Income.InsertIncome, dynamicParameters, commandType: CommandType.StoredProcedure);
                return dynamicParameters.Get<int>("@Id");
            });

            return result;
        }

        public async Task<bool> UpdateAsync(Income data)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async conn =>
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { data.Name, data.Amount, data.Description, UpdatedDate = DateTime.UtcNow });
                var dbResult = await conn.ExecuteAsync(StoredProcedures.Expense.UpdateExpense, dynamicParameters, commandType: CommandType.StoredProcedure);

                return dbResult != 0;
            });

            return result;
        }
    }
}