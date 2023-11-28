using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Channels;
using IntegrationService.Models.Product;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class ChannelProductStoreRepository :
    GenericRepository<ChannelProductStore>, IChannelProductStoreRepository
    {
        public ChannelProductStoreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
