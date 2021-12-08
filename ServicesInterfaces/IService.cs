using System.Threading.Tasks;

namespace ServicesInterfaces {
    public interface IService<T> {
        // T - DTO
        Task<T> GetAsync(int? id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(int id);
        bool ItemExists(int id);
    }
}