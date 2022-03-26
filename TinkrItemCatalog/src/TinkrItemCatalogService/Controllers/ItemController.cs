using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TinkrCatalogContracts;
using TinkrCommon;
using TinkrItemCatalogService.Dtos;
using TinkrItemCatalogService.Entities;
using TinkrItemCatalogService.Extensions;

namespace TinkrItemCatalogService.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController:ControllerBase
    {
        private readonly IRepository<Item> _repository;
        private IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Item> repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItemsAsync()
        {
            var items = (await _repository.GetAsyncAll())
                                            .Select(item => item.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetAsyncItem(Guid id)
        {
            var item = await _repository.GetAsyncById(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreationDate = DateTimeOffset.UtcNow
            };
            await _repository.CreateAsync(item);
            await _publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));
            return CreatedAtAction(nameof(GetAsyncItem), new {id = item.Id},
                item.AsDto());
        }

        [HttpPut("{id}")]
        public async Task <ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _repository.GetAsyncById(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            var updatedItem = existingItem with
            {
                Name = itemDto.Name, Description = itemDto.Description, Price = itemDto.Price
            };
            await _repository.UpdateAsync(updatedItem);
            await _publishEndpoint.Publish(new CatalogItemUpdated(updatedItem.Id, updatedItem.Name, updatedItem.Description));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            var existingItem = await _repository.GetAsyncById(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            await _publishEndpoint.Publish(new CatalogItemDeleted(existingItem.Id));
            return NoContent();
        }
    }
}