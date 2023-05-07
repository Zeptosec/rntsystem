using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Exceptions;
using RentSystem.Core.Policies;
using System.Security.Claims;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IItemRepository _itemRepository;
        private readonly IAdvertRepository _advertRepository;
        private readonly IValidator<ItemDTO> _validator;
        public ItemController(IItemService itemService, IUserService userService, 
                              IAuthorizationService authorizationService, IItemRepository itemRepository, 
                              IAdvertRepository advertRepository, IValidator<ItemDTO> validator)
        {
            _itemService = itemService;
            _itemRepository = itemRepository;
            _userService = userService;
            _advertRepository = advertRepository;
            _authorizationService = authorizationService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _itemService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _itemService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemDTO itemDTO)
        {
            var userId = GetUserId();

            var user = await _userService.GetAsync(userId) ?? throw new NotFoundException("User was not found");

            var result = _validator.Validate(itemDTO);

            if (result.IsValid)
            {
                await _itemService.CreateAsync(itemDTO, user.Id);
                return Ok();
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ItemDTO itemDTO)
        {
            var result = _validator.Validate(itemDTO);

            var item = await _itemRepository.GetAsync(id);
            var advert = await _advertRepository.GetAsync(itemDTO.AdvertId);

            var itemAuthResult = await _authorizationService.AuthorizeAsync(User, item, PolicyNames.SameUser);
            var advertAuthResult = await _authorizationService.AuthorizeAsync(User, advert, PolicyNames.SameUser);

            if (!itemAuthResult.Succeeded || !advertAuthResult.Succeeded)
            {
                return Forbid();
            }

            if (result.IsValid)
            {
                await _itemService.UpdateAsync(id, itemDTO);
                return Ok();
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemRepository.GetAsync(id);

            var authResult = await _authorizationService.AuthorizeAsync(User, item, PolicyNames.SameUser);

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            await _itemService.DeleteAsync(id);

            return Ok();
        }
    }
}