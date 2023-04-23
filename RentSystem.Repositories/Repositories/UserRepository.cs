using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;

namespace RentSystem.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RentDBContext _rentDBContext;

        public UserRepository(RentDBContext rentDBContext) 
        {
            _rentDBContext = rentDBContext;
        }

        public async Task CreateAsync(User item)
        {
            _rentDBContext.Users.Add(item);

            await _rentDBContext.SaveChangesAsync();
        }

        public Task DeleteAsync(User item)
        {
            throw new NotImplementedException();
        }

        public User? FirstOrDefault(Func<User, bool> predicate)
        {
            return _rentDBContext.Users.FirstOrDefault(predicate);
        }

        public Task<ICollection<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetAsync(int id)
        {
            return await _rentDBContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task UpdateAsync(User item)
        {
            throw new NotImplementedException();
        }
    }
}
