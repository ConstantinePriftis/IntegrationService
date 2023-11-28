using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Channels;
using IntegrationService.Models.Fields;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class ChannelRepository :
    GenericRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
