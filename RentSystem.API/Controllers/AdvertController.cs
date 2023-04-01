using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/adverts")]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        private readonly IValidator<AdvertDTO> _validator;
        public AdvertController(IAdvertService advertService, IValidator<AdvertDTO> validator)
        {
            _advertService = advertService;
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
        public async Task<IActionResult> Create(AdvertDTO advertDTO)
        {
            var result = _validator.Validate(advertDTO);

            if (result.IsValid)
            {
                await _advertService.CreateAsync(advertDTO);
                return Ok();
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AdvertDTO advertDTO)
        {
            var result = _validator.Validate(advertDTO);

            if (result.IsValid)
            {
                await _advertService.UpdateAsync(id, advertDTO);

                return Ok();
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _advertService.DeleteAsync(id);

            return Ok();
        }
    }
}
