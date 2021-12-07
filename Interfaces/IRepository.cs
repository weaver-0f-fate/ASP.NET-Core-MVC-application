using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces {
    public interface IRepository<T> : IDisposable {
        Task<IEnumerable<T>> GetEntityList();
        Task<T> GetEntity(int id);
        Task Create(T item);
        Task Update(T item);
        Task Delete(int id);
        Task Save();

    }
}