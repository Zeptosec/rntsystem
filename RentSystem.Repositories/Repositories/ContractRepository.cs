using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;

namespace RentSystem.Repositories.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly RentDBContext _rentDBContext;

        public ContractRepository(RentDBContext rentDBContext)
        {
            _rentDBContext = rentDBContext;
        }

        public async Task<ICollection<Contract>> GetAllAsync()
        {
            return await _rentDBContext.Contracts.Include(x => x.Renter).Include(x => x.Owner).Include(x => x.Item).ToListAsync();
        }

        public async Task<Contract?> GetAsync(int id)
        {
            return await _rentDBContext.Contracts.Include(x => x.Renter).Include(x => x.Owner).Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Contract contract)
        {
            _rentDBContext.Contracts.Add(contract);

            await _rentDBContext.SaveChangesAsync();
        }
    }
}
