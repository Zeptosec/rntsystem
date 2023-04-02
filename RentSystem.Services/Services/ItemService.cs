﻿using AutoMapper;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;

namespace RentSystem.Services.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAdvertRepository _advertRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IAdvertRepository advertRepository, IMapper mapper) 
        {
            _itemRepository = itemRepository;
            _advertRepository = advertRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetItemDTO>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllAsync();

            return _mapper.Map<List<GetItemDTO>>(items);
        }

        public async Task<GetItemDTO> GetAsync(int id)
        {
            var item = await _itemRepository.GetAsync(id);

            if (item == null)
            {
                throw new Exception();
            }

            return _mapper.Map<GetItemDTO>(item);
        }

        public async Task CreateAsync(ItemDTO itemDTO)
        {
            var item = _mapper.Map<Item>(itemDTO);

            await _itemRepository.CreateAsync(item);
        }

        public async Task UpdateAsync(int id, ItemDTO itemDTO)
        {
            var item = await _itemRepository.GetAsync(id);

            if (item == null)
            {
                throw new Exception();
            }

            item.Name = itemDTO.Name;
            item.Price = itemDTO.Price;
            item.Category = itemDTO.Category;
            item.State = itemDTO.State;

            var advert = await _advertRepository.GetAsync(itemDTO.AdvertId);

            if (advert == null) throw new Exception("No such advert exists");

            item.AdvertId = advert.Id;

            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _itemRepository.GetAsync(id);

            if (item == null)
            {
                throw new Exception();
            }

            await _itemRepository.DeleteAsync(item);
        }
    }
}
