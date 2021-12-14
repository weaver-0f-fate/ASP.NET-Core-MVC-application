using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesInterfaces {
    public interface IService<TDto> {
        Task<IEnumerable<TDto>> GetAllItemsAsync(string searchString = null, int? filter = null);
        Task<TDto> GetAsync(int? id);
        Task<TDto> CreateAsync(TDto item);
        Task<TDto> UpdateAsync(TDto item);
        Task DeleteAsync(int id);
    }
}