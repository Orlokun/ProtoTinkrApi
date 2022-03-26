using System;
using TinkrCommon;

namespace TinkrItemsInventoryService.Entities{
    public class InventoryItem : IEntity
    {
        public Guid Id { get; init; }

        public Guid UserId {get; init;}

        public Guid CatalogItemId { get; init; }
    
        public int Quantity { get; set; }

        public DateTimeOffset AcquiredDate{get;init;}
    }
}