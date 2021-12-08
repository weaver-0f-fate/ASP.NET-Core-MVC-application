using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Interfaces;
using ServicesInterfaces;

namespace Services.Services {
    public abstract class AbstractService<T, TT> : IService<TT> {
        //T - Domain object, TT - DTO
        protected readonly IRepository<T> _repository;
        protected readonly IMapper _mapper;

        public AbstractService(IRepository<T> repository, IMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }

        public abstract Task<IEnumerable<TT>> GetAllItemsAsync(string searchString = null, string filter = null);

        public abstract Task<IEnumerable<string>> GetNames();

        public abstract Task<TT> GetAsync(int? id);

        public abstract Task CreateAsync(TT item);

        public abstract Task UpdateAsync(TT item);

        public abstract Task DeleteAsync(int id);

        public async Task<bool> ItemExists(int id) {
            return await _repository.Exists(id);
        }
    }
}