using RentSystem.Core.DTOs;
using RentSystem.Core.Enums;

namespace RentSystem.Core.Contracts.Service
{
    public interface IItemService
    {
        Task<ICollection<GetItemDTO>> GetAllAsync(Category? category);
        Task<GetItemDTO> GetAsync(int id);
        Task CreateAsync(ItemDTO item, int userId);
        Task UpdateAsync(int id, ItemDTO item);
        Task DeleteAsync(int id);
    }
}
