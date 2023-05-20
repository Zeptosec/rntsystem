using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Repositories.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly RentDBContext _rentDBContext;

        public ItemRepository(RentDBContext rentDBContext) 
        {
            _rentDBContext = rentDBContext;
        }

        public async Task<ICollection<Item>> GetAllAsync(Category? category)
        {
            if (category.HasValue) 
            {
                return await _rentDBContext.Items.Where(x => x.Category == category).Include(x => x.User).Include(x => x.Reservation).ToListAsync();
            }

            return await _rentDBContext.Items.Include(x => x.User).Include(x => x.Reservation).ToListAsync();
        }

        public async Task<Item?> GetAsync(int id)
        {
            return await _rentDBContext.Items.Include(x => x.User).Include(x => x.Reservation).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task CreateAsync(Item item)
        {
            _rentDBContext.Items.Add(item);

            await _rentDBContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Item item)
        {
            _rentDBContext.Items.Update(item);

            await _rentDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item item)
        {
            _rentDBContext.Items.Remove(item);

            await _rentDBContext.SaveChangesAsync();
        }
    }
}
