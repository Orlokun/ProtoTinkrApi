using System;
using System.ComponentModel.DataAnnotations;

namespace TinkrItemCatalogService.Dtos
{
    public record ItemDto (Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreationDate); 
    public record CreateItemDto ([Required] Guid Id, string Name, string Description, [Range(1, 1000)] decimal Price); 
    public record UpdateItemDto (Guid Id, string Name, string Description, [Range(1, 1000)] decimal Price); 
}