using System.Threading.Tasks;
using MassTransit;
using TinkrCatalogContracts;
using TinkrCommon;
using TinkrItemsInventoryService.Entities;

namespace TinkerInventoryService.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsyncById(message.itemId);
            if (item is null)
            {
                return;
            }
            else
            {
                await _repository.DeleteAsync(message.itemId);
            }
        }
    }
}