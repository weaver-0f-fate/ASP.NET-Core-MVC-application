using System;
using System.Collections.Generic;
using Interfaces;

namespace Data {
    public abstract class AbstractRepository<T> :IRepository<T> {
        protected readonly Task9Context _context;
        private bool _disposed;

        protected AbstractRepository(Task9Context context) {
            _context = context;
        }

        public abstract IEnumerable<T> GetEntityList();
        public abstract IEnumerable<T> GetEntityList(string searchString);
        public abstract T GetEntity(int id);

        public void Create(T item) {
            if (item is null) {
                return;
            }
            _context.Add(item);
            Save();
        }

        public void Update(T item) {
            if (item is null) {
                return;
            }
            _context.Update(item);
            Save();
        }
        public abstract void Delete(int id);

        public void Save() {
            _context.SaveChanges();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

    }
}
