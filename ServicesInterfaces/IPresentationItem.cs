using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesInterfaces {
    public interface IPresentationItem<T, TT> {
        // T - Domain Object, TT - DTO
        Task<IEnumerable<TT>> GetAllItems(string searchString);
        Task<TT> GetItem(int? id);
        Task CreateItem(TT item);
        Task UpdateItem(TT item);
        Task DeleteItem(int id);
        bool ItemExists(int id);
    }
}