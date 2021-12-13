using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesInterfaces {
    public interface IService<Tdto> {
        Task<IEnumerable<Tdto>> GetAllItemsAsync(string searchString = null, string filter = null);
        Task<Tdto> GetAsync(int? id);
        Task<Tdto> CreateAsync(Tdto item);
        Task<Tdto> UpdateAsync(Tdto item);
        Task DeleteAsync(int id);
        Task<bool> ItemExists(int id);
    }
}