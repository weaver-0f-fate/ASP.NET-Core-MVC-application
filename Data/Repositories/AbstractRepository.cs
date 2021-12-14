using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public abstract class AbstractRepository<T> :IRepository<T> where T : AbstractModel {
        protected readonly Task9Context Context;
        private readonly DbSet<T> _repository;
        private bool _disposed;

        protected AbstractRepository(Task9Context context, DbSet<T> repo) {
            Context = context;
            _repository = repo;
        }

        public abstract Task<IEnumerable<T>> GetEntityListAsync();

        public async Task<T> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }
            var item = await GetItemAsync(id);
            if (item is null) {
                throw new NoEntityException();
            }
            return item;
        }

        public async Task CreateAsync(T item) {
            if (item is null) {
                return;
            }
            await Context.AddAsync(item);
            await SaveAsync();
        }

        public async Task UpdateAsync(T item) {
            if (item is null) {
                return;
            }
            Context.Update(item);
            await SaveAsync();
        }

        public virtual async Task DeleteAsync(int id) {
            var item = await GetEntityAsync(id);
            _repository.Remove(item);
            await SaveAsync();
        }

        public async Task SaveAsync() {
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) {
            return await _repository.AnyAsync(e => e.Id == id);
        }

        public async void Dispose() {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        private async Task DisposeAsync(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    await Context.DisposeAsync();
                }
            }
            _disposed = true;
        }

        protected abstract Task<T> GetItemAsync(int id);
    }
}
