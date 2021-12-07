using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesInterfaces {
    public interface IPresentationItem<TT> {
        // TT - DTO
        Task<TT> GetItem(int? id);
        Task CreateItem(TT item);
        Task UpdateItem(TT item);
        Task DeleteItem(int id);
        bool ItemExists(int id);
    }
}