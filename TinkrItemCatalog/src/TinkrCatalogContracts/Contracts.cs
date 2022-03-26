using System;

namespace TinkrCatalogContracts
{
    public record CatalogItemCreated (Guid ItemId, string Name, string Description);

    public record CatalogItemUpdated (Guid itemId, string Name, string Description);
    
    public record CatalogItemDeleted (Guid itemId);
}