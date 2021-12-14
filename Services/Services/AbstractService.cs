using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;

namespace Services.Services {
    public abstract class AbstractService<TModel, TDto> : IService<TDto> where TModel : AbstractModel where TDto : AbstractDto{
        protected readonly IRepository<TModel> Repository;
        private readonly IMapper _mapper;

        protected AbstractService(IRepository<TModel> repository, IMapper mapper) {
            Repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetAllItemsAsync(FilteringParameters parameters = null) {
            var items = await GetFilteredItemsAsync(parameters);
            return items.Select(x => _mapper.Map<TDto>(x));
        }

        public async Task<TDto> GetAsync(int? id) {
            if (id is null) {
                throw new NoEntityException();
            }

            var item = await Repository.GetEntityAsync((int)id);
            if (item is null) {
                throw new NoEntityException();
            }
            return _mapper.Map<TDto>(item);
        }
        public async Task<TDto> CreateAsync(TDto itemDto) {
            var item = _mapper.Map<TModel>(itemDto);
            await Repository.CreateAsync(item);
            return itemDto;
        }
        public async Task<TDto> UpdateAsync(TDto itemDto) {
            var item = _mapper.Map<TModel>(itemDto);
            await Repository.UpdateAsync(item);
            return itemDto;
        }
        public virtual async Task DeleteAsync(int id) {
            await Repository.DeleteAsync(id);
        }

        protected abstract Task<List<TModel>> GetFilteredItemsAsync(FilteringParameters parameters);
    }
}