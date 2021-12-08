using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public abstract Task<T> GetEntityAsync(int id);

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
        public abstract Task DeleteAsync(int id);

        public async Task SaveAsync() {
            await Context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id) {
            return await _repository.AnyAsync(e => e.Id == id);
        }

        public async void Dispose() {
            await Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    await Context.DisposeAsync();
                }
            }
            _disposed = true;
        }

    }
}
