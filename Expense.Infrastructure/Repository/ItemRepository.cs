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
    public class ItemRepository : IItemRepository
    {
        public async Task<List<Item>> GetAllAsync()
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var result = await con.QueryAsync<Item>("select * from Item", commandType: CommandType.Text);
                return result.ToList();
            };
        }

        public async Task<int> InsertAsync(Item item)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { item.Name, item.Description, CreatedDate = DateTime.UtcNow });
                await con.ExecuteAsync(StoredProcedures.Item.InsertItem, dynamicParameters, commandType: CommandType.StoredProcedure);
                return dynamicParameters.Get<int>("@Id");
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int dbResult = await con.ExecuteAsync("delete from Item where Id = @Id", new { Id = id }, commandType: CommandType.Text);
                return dbResult != 0;
            };
        }

        public async Task<bool> UpdateAsync(Item item)
        {
            using (var con = new SqlConnection(ConfigHelper.ConnnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { item.Id, item.Name, item.Description, UpdatedDate = DateTime.UtcNow });
                var dbResult = await con.ExecuteAsync(StoredProcedures.Item.UpdateItem, dynamicParameters, commandType: CommandType.StoredProcedure);

                return dbResult != 0;
            };
        }
    }
}