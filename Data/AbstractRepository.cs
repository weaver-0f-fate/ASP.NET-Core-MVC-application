using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;

namespace Data {
    public abstract class AbstractRepository<T> :IRepository<T> {
        protected readonly Task9Context _context;
        private bool _disposed;

        protected AbstractRepository(Task9Context context) {
            _context = context;
        }

        public abstract Task<IEnumerable<T>> GetEntityList();
        public abstract Task<T> GetEntity(int id);

        public async Task Create(T item) {
            if (item is null) {
                return;
            }
            await _context.AddAsync(item);
            await Save();
        }

        public async Task Update(T item) {
            if (item is null) {
                return;
            }
            _context.Update(item);
            await Save();
        }
        public abstract Task Delete(int id);

        public async Task Save() {
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
