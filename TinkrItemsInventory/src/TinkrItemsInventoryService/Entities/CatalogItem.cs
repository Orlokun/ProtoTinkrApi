using System;
using TinkrCommon;

namespace TinkrItemsInventoryService.Entities{
    public class CatalogItem : IEntity
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}