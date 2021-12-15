using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;

namespace Services {
    public interface IService<TModel, TDto> where TDto : AbstractDto where TModel : AbstractModel{
        Task<IEnumerable<TDto>> GetAllItemsAsync(IFilter<TModel> filter);
        Task<TDto> GetAsync(int? id);
        Task<TDto> CreateAsync(TDto item);
        Task<TDto> UpdateAsync(TDto item);
        Task DeleteAsync(int id);
    }
}