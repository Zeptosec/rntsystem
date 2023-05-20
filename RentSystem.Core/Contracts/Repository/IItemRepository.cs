using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Core.Contracts.Repository
{
    public interface IItemRepository
    {
        Task<ICollection<Item>> GetAllAsync(Category? category);
        Task<Item?> GetAsync(int id);
        Task CreateAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(Item item);
    }
}
