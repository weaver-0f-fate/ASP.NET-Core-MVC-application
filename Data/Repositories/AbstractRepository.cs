using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;

namespace Data.Repositories {
    public abstract class AbstractRepository<T> :IRepository<T> {
        protected readonly Task9Context _context;
        private bool _disposed;

        protected AbstractRepository(Task9Context context) {
            _context = context;
        }

        public abstract Task<IEnumerable<T>> GetEntityListAsync();
        public abstract Task<T> GetEntityAsync(int id);

        public async Task CreateAsync(T item) {
            if (item is null) {
                return;
            }
            await _context.AddAsync(item);
            await SaveAsync();
        }

        public async Task UpdateAsync(T item) {
            if (item is null) {
                return;
            }
            _context.Update(item);
            await SaveAsync();
        }
        public abstract Task DeleteAsync(int id);

        public async Task SaveAsync() {
            await _context.SaveChangesAsync();
        }

        public async void Dispose() {
            await Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    await _context.DisposeAsync();
                }
            }
            _disposed = true;
        }

    }
}
