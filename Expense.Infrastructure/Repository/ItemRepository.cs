using System;
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
    public class ItemRepository : IItemRepository
    {
        public async Task<List<Item>> GetAllAsync()
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, con =>
            {
                return con.QueryAsync<Item>("select * from Item", commandType: CommandType.Text);
            });

            return result.ToList();
        }

        public async Task<int> InsertAsync(Item item)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async con =>
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                dynamicParameters.AddDynamicParams(new { item.Name, item.Description, CreatedDate = DateTime.UtcNow });
                await con.ExecuteAsync(StoredProcedures.Item.InsertItem, dynamicParameters, commandType: CommandType.StoredProcedure);
                return dynamicParameters.Get<int>("@Id");
            });

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async conn =>
            {
                int dbResult = await conn.ExecuteAsync("delete from Item where Id = @Id", new { Id = id }, commandType: CommandType.Text);
                return dbResult != 0;
            });

            return result;
        }

        public async Task<bool> UpdateAsync(Item item)
        {
            var result = await ConnectionHelper.Connect(ConfigHelper.ConnnectionString, async conn =>
           {
               DynamicParameters dynamicParameters = new DynamicParameters();
               dynamicParameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
               dynamicParameters.AddDynamicParams(new { item.Id, item.Name, item.Description, UpdatedDate = DateTime.UtcNow });
               var dbResult = await conn.ExecuteAsync(StoredProcedures.Item.UpdateItem, dynamicParameters, commandType: CommandType.StoredProcedure);

               return dbResult != 0;
           });

            return result;
        }
    }
}