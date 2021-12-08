using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces {
    public interface IRepository<T> : IDisposable {
        Task<IEnumerable<T>> GetEntityListAsync();
        Task<T> GetEntityAsync(int id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(int id);
        Task SaveAsync();
        Task<bool> Exists(int id);

    }
}