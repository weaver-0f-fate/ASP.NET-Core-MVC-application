using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;
using ServicesInterfaces;

namespace Services.Services {
    public abstract class AbstractService<TModel, TDto> : IService<TDto> where TModel : AbstractModel where TDto : AbstractDto{
        protected readonly IRepository<TModel> Repository;
        protected readonly IMapper Mapper;

        protected AbstractService(IRepository<TModel> repository, IMapper mapper) {
            Repository = repository;
            Mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetAllItemsAsync(string searchString = null, string filter = null) {
            var items = await GetFilteredItems(searchString, filter);
            return items.Select(x => Mapper.Map<TDto>(x));
        }

        public async Task<TDto> GetAsync(int? id) {
            if (id is null) {
                throw new NoEntityException();
            }

            var item = await Repository.GetEntityAsync((int)id);
            if (item is null) {
                throw new NoEntityException();
            }
            return Mapper.Map<TDto>(item);
        }
        public async Task<TDto> CreateAsync(TDto itemDto) {
            var item = Mapper.Map<TModel>(itemDto);
            await Repository.UpdateAsync(item);
            return itemDto;
        }
        public async Task<TDto> UpdateAsync(TDto itemDto) {
            var item = Mapper.Map<TModel>(itemDto);
            await Repository.UpdateAsync(item);
            return itemDto;
        }
        public virtual async Task DeleteAsync(int id) {
            await Repository.DeleteAsync(id);
        }
        public async Task<bool> ItemExistsAsync(int id) {
            return await Repository.ExistsAsync(id);
        }

        protected abstract Task<List<TModel>> GetFilteredItems(string searchString = null, string filter = null);
    }
}