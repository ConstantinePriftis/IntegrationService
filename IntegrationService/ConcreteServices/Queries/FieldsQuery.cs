using IntegrationService.IServices.IQuery;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.FieldViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace IntegrationService.ConcreteServices.Query
{
    public class FieldsQuery : IFieldsQuery
    {
        private readonly IFieldRepository _fieldRepository;
        public FieldsQuery(IFieldRepository fieldRepository)
        {
            _fieldRepository= fieldRepository;
        }
        public async Task<List<FieldViewModel>> GetListAsync()
        {
            return await GetFieldsListQueryable().ToListAsync();
        }
        public async Task<List<FieldViewModel>> GetListAsync(Expression<Func<FieldViewModel, bool>> predicate)
        {
            return await GetFieldsListQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public Task<PagedResult<FieldViewModel>> GetPagedListAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<FieldViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            throw new NotImplementedException();
        }

        public Task<List<SelectListItem>> GetSelectListItems()
        {
            throw new NotImplementedException();
        }
       
        private IQueryable<FieldViewModel> GetFieldsListQueryable()
        {
            return (from fields in _fieldRepository.GetAll().Include(x => x.DOMGroups)
                    //join fieldValues in 
                    select new FieldViewModel
                    {
                        Id = fields.Id,
                        Name = fields.Name,
                        Type = fields.Type,
                        DomGroupId = fields.DOMGroupsId,
                        PredefinedValues = fields.DOMGroups.DOMValues.Select(x=>x.Value).ToList(),
                        //Channels = fields.FieldChannels.Select(x => x.Channel).Select(x => x.ToDto()).ToList()
                    }
                   );
        }

        Task<PagedResult<FieldViewModel>> IQuery<FieldViewModel>.GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            throw new NotImplementedException();
        }
    }
}
