using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Interfaces;
using ServicesInterfaces;

namespace Services.Services {
    public abstract class AbstractService<T, TT> : IService<TT> where T : AbstractModel where TT : IDTO{
        //T - Domain object, TT - DTO
        protected readonly IRepository<T> Repository;
        protected readonly IMapper Mapper;

        protected AbstractService(IRepository<T> repository, IMapper mapper) {
            Repository = repository;
            Mapper = mapper;
        }

        public async Task<IEnumerable<TT>> GetAllItemsAsync(string searchString = null, string filter = null) {
            var items = await GetFilteredItems(searchString, filter);
            return items.Select(x => Mapper.Map<TT>(x));
        }

        public abstract Task<IEnumerable<string>> GetNames();

        public async Task<TT> GetAsync(int? id) {
            if (id is null) {
                throw new NoEntityException();
            }

            var item = await Repository.GetEntityAsync((int)id);
            if (item is null) {
                throw new NoEntityException();
            }
            return Mapper.Map<TT>(item);
        }
        public async Task CreateAsync(TT itemDto) {
            var item = Mapper.Map<T>(itemDto);
            await Repository.UpdateAsync(item);
        }
        public async Task UpdateAsync(TT itemDto) {
            var item = Mapper.Map<T>(itemDto);
            await Repository.UpdateAsync(item);
        }
        public virtual async Task DeleteAsync(int id) {
            await Repository.DeleteAsync(id);
        }
        public async Task<bool> ItemExists(int id) {
            return await Repository.ExistsAsync(id);
        }

        protected abstract Task<List<T>> GetFilteredItems(string searchString = null, string filter = null);
    }
}