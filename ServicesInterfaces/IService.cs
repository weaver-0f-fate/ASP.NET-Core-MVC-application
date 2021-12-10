using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesInterfaces {
    public interface IService<T> {
        // T - DTO
        Task<IEnumerable<T>> GetAllItemsAsync(string searchString = null, string filter = null);
        Task<T> GetAsync(int? id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(int id);
        Task<bool> ItemExists(int id);
    }
}