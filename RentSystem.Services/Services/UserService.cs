using AutoMapper;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Exceptions;
using RentSystem.Core.Extensions;

namespace RentSystem.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenManager _tokenManager;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, ITokenManager tokenManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenManager = tokenManager;
            _mapper = mapper;
        }
        public async Task CreateAsync(RegisterUserDTO userDTO)
        {
            var user = _userRepository.FirstOrDefault(x => x.Email == userDTO.Email);

            if (user != null) throw new BadRequestException("User with this email already exists");

            var newUser = _mapper.Map<User>(userDTO);

            await _userRepository.CreateAsync(newUser);
        }

        public Task DeleteAsync(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO?> GetAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);

            if(user == null) throw new NotFoundException("User", id);
            
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<SuccessfullLoginDTO> LoginAsync(LoginUserDTO userDTO)
        {
            var user = _userRepository.FirstOrDefault(x => x.Email == userDTO.Email);

            if (user == null) throw new BadRequestException("Email or password is invalid");

            if (!user.Verify(userDTO.Password)) throw new BadRequestException("Email or password is invalid");

            var accessToken = _tokenManager.CreateAccessTokenAsync(user);

            return new SuccessfullLoginDTO { AccessToken = accessToken };
        }

        public Task UpdateAsync(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }
    }
}
