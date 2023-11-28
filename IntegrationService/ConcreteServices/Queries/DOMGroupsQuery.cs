using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IQuery;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.FieldViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.Queries
{
    public class DOMGroupsQuery : IDOMGroupsQuery
    {
        private readonly IDOMGroupsRepository _DOMGroupsRepository;
        public DOMGroupsQuery(IDOMGroupsRepository dOMGroupsRepository)
        {
            _DOMGroupsRepository = dOMGroupsRepository;
        }
        public Task<List<DOMGroupsViewModel>> GetListAsync(Expression<Func<DOMGroupsViewModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DOMGroupsViewModel>> GetListAsync()
        {
            return await GetDOMGroupsListQueryable().ToListAsync();
        }

        public Task<PagedResult<DOMGroupsViewModel>> GetPagedListAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<DOMGroupsViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            throw new NotImplementedException();
        }

        public Task<List<SelectListItem>> GetSelectListItems()
        {
            throw new NotImplementedException();
        }

        private IQueryable<DOMGroupsViewModel> GetDOMGroupsListQueryable()
        {
            return (from domGroup in _DOMGroupsRepository.GetAll().Include(x => x.Fields).Include(x=>x.DOMValues)
                    select new DOMGroupsViewModel()
                    {
                        Id = domGroup.Id,
                        Description = domGroup.Description,
                        Fields = domGroup.Fields.Select(x => x.ToDto()).ToList(),
                        DOMValues = domGroup.DOMValues.Select(x => x.ToDto()).ToList()
                    }
                   );
        }
    }
}
