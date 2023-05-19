using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _userService.GetAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserDTO userDTO)
        {
            var userId = GetUserId();

            await _userService.UpdateAsync(userId, userDTO);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var userId = GetUserId();

            await _userService.DeleteAsync(userId);
            return NoContent();
        }
    }
}