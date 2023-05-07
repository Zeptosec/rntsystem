using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentSystem.API.Extensions;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Enums;
using RentSystem.Core.Policies;
using System.Security.Claims;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/adverts")]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        private readonly IAdvertRepository _advertRepository;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IValidator<AdvertDTO> _validator;

        public AdvertController(IAdvertService advertService, IUserService userService, IAdvertRepository advertRepository, IAuthorizationService authorizationService, IValidator<AdvertDTO> validator)
        {
            _advertService = advertService;
            _advertRepository = advertRepository;
            _userService = userService;
            _authorizationService = authorizationService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _advertService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _advertService.GetAsync(id));
        }

        [HttpPost]
        [AuthorizeRole(Role.Owner)]
        public async Task<IActionResult> Create(AdvertDTO advertDTO)
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userService.GetAsync(int.Parse(userId)) ?? throw new Exception("User not found");

            var result = _validator.Validate(advertDTO);

            if (result.IsValid)
            {
                await _advertService.CreateAsync(advertDTO, user.Id);
                return Ok();
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPut("{id}")]
        [AuthorizeRole(Role.Owner)]
        public async Task<IActionResult> Update(int id, AdvertDTO advertDTO)
        {
            var result = _validator.Validate(advertDTO);

            var advert = await _advertRepository.GetAsync(id);

            var authResult = await _authorizationService.AuthorizeAsync(User, advert, PolicyNames.SameUser);

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            if (result.IsValid)
            {
                await _advertService.UpdateAsync(id, advertDTO);

                return Ok();
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpDelete]
        [AuthorizeRole(Role.Owner)]
        public async Task<IActionResult> Delete(int id)
        {
            var advert = await _advertRepository.GetAsync(id);

            var authResult = await _authorizationService.AuthorizeAsync(User, advert, PolicyNames.SameUser);

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            await _advertService.DeleteAsync(id);

            return Ok();
        }
    }
}
