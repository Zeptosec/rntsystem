using RentSystem.Core.DTOs;


namespace RentSystem.Core.Contracts.Service
{
    public interface IUserService
    {
        Task<ICollection<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetAsync(int id);
        Task<SuccessfullLoginDTO> LoginAsync(LoginUserDTO userDTO);
        Task CreateAsync(RegisterUserDTO userDTO);
        Task UpdateAsync(UserDTO userDTO);
        Task DeleteAsync(UserDTO userDTO);
    }
}
