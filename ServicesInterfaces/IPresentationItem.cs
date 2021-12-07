using System.Threading.Tasks;

namespace ServicesInterfaces {
    public interface IPresentationItem<TT> {
        // TT - DTO
        Task<TT> GetItemAsync(int? id);
        Task CreateItemAsync(TT item);
        Task UpdateItemAsync(TT item);
        Task DeleteItemAsync(int id);
        bool ItemExists(int id);
    }
}