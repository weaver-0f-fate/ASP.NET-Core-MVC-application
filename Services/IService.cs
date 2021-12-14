using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services {
    public interface IService<TDto> {
        Task<IEnumerable<TDto>> GetAllItemsAsync(FilteringParameters parameters);
        Task<TDto> GetAsync(int? id);
        Task<TDto> CreateAsync(TDto item);
        Task<TDto> UpdateAsync(TDto item);
        Task DeleteAsync(int id);
    }
}