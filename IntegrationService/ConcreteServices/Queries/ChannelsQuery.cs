using IntegrationService.IServices.IQuery;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.Models.Channels;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.ChannelViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.Query
{
    public class ChannelsQuery : IChannelsQuery
    {
        private readonly IChannelRepository _channelRepository;
        public ChannelsQuery(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }
        public async Task<List<ChannelViewModel>> GetListAsync(Expression<Func<ChannelViewModel, bool>> predicate)
        {
            return await GetChannelsListQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<List<ChannelViewModel>> GetListAsync()
        {
            return await GetChannelsListQueryable().ToListAsync();
        }

        public Task<PagedResult<ChannelViewModel>> GetPagedListAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ChannelViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            return await GetChannelsListQueryable().Select(x=>x.ToSelectListItem()).ToListAsync();
        }
        private IQueryable<ChannelViewModel> GetChannelsListQueryable()
        {
            return (from channel in _channelRepository.GetAll()//.Include(x => x.FieldChannels)
                    select new ChannelViewModel()
                    {
                        Id = channel.Id,
                        Name = channel.ChannelName,
                        Description = channel.Description,
                        //Fields = channel.FieldChannels.Select(x => x.Field).Select(x => x.ToDto()).ToList()
                    }
                   );
        }
    }
}
