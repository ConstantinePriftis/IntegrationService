using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Categories;
using IntegrationService.Models.Imports;
using IntegrationService.Models.User;

namespace IntegrationService.ConcreteServices.Commands
{
    public class CategoryCommand : ICategoryCommand
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryCommand(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public Task<int> Insert(Category item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, Category item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }
        public virtual void InsertRange(List<Category> categories)
        {
            _categoryRepository.AddRange(categories);
            _categoryRepository.Save();
        }

        public void InsertFromImport(List<Category> collection)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertRange(IEnumerable<Category> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
