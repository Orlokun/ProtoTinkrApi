using System;
using TinkrCommon;

namespace TinkrItemCatalogService.Entities{
    public record Item : IEntity
    {
        public Guid Id {get; init;}
        public string Name {get; set;}
        public string Description {get; set;}
        public decimal Price {get; set;}
        public DateTimeOffset CreationDate { get; set; }
    }
}