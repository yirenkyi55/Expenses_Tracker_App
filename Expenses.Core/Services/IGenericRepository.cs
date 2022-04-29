using System.Collections.Generic;
using System.Threading.Tasks;

using Expenses.Core.Domain;

namespace Expenses.Core.Services
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();

        Task<int> InsertAsync(T data);

        Task<bool> UpdateAsync(T data);

        Task<bool> DeleteAsync(int id);
    }
}