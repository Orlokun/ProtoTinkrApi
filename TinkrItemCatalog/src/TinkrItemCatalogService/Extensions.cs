using System;
using TinkrItemCatalogService.Dtos;
using TinkrItemCatalogService.Entities;

namespace TinkrItemCatalogService.Extensions
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreationDate);
        }
    }
}