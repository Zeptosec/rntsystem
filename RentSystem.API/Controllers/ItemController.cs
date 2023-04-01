using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IValidator<ItemDTO> _validator;
        public ItemController(IItemService itemService, IValidator<ItemDTO> validator)
        {
            _itemService = itemService;
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
            var result = _validator.Validate(itemDTO);

            if (result.IsValid)
            {
                await _itemService.CreateAsync(itemDTO);
                return Ok(); 
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ItemDTO itemDTO)
        {
            var result = _validator.Validate(itemDTO);

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
            await _itemService.DeleteAsync(id);

            return Ok();
        }
    }
}